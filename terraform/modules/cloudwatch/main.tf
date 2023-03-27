resource "aws_cloudwatch_log_group" "backoffice_ecs" {
  name = "droits-backoffice-ecs-lg"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_stream" "backoffice" {
  name           = "droits-backoffice-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.backoffice_ecs.name
}


resource "aws_cloudwatch_log_group" "webapp_ecs" {
  name = "droits-webapp-ecs-lg"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_stream" "webapp" {
  name           = "droits-webapp-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.webapp_ecs.name
}

# to-do: fill this in using best practices. Assume we'll want both containers and the DB
resource "aws_cloudwatch_dashboard" "droits" {
  dashboard_name = "droits-${terraform.workspace}"

  dashboard_body = jsonencode({
    # search backoffice logs for droits that failed to be reported
    widgets = [
        {
          "height": 6,
          "width": 24,
          "y": 2,
          "x": 0,
          "type": "log",
          "properties": {
              "query": "SOURCE '/${aws_cloudwatch_log_group.backoffice_ecs.name}' | fields @timestamp, @message\n| sort @timestamp desc\n| filter @message like /Failed to report/",
              "region": "eu-west-2",
              "stacked": false,
              "view": "table"
          }
      },
    # search backoffice logs for all attempts to report a droit
      {
          "type": "log",
          "x": 0,
          "y": 8,
          "width": 24,
          "height": 6,
          "properties": {
              "query": "SOURCE '/${aws_cloudwatch_log_group.backoffice_ecs.name}' | fields @timestamp, @message\n| fields strcontains(@message, \"DROIT reported\") as report_succeeded\n| fields strcontains(@message, \"Failed to report\") as report_failed\n| stats sum(report_succeeded) as reports_succeeded, sum(report_failed) as reports_failed by bin(1d) as day\n| sort day asc",
              "region": "eu-west-2",
              "stacked": false,
              "title": "Attempted wreck reports per day",
              "view": "bar"
          }
      },
      {
        type   = "metric"
        x      = 0
        y      = 0
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ECS",
              "CPUUtilization",
              "ClusterName",
              "${var.ecs_cluster_name}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = "eu-west-2"
          title  = "${var.ecs_cluster_name} ECS cluster CPU utilistation"
        }
      },
      {
        type   = "text"
        x      = 0
        y      = 7
        width  = 3
        height = 3

        properties = {
          markdown = "Welcome to the DROITS dashboard!"
        }
      }
    ]
  })
}