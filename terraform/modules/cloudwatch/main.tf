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

# only health metrics e.g cpu utilisation and requests made
# want both containers and the DB
resource "aws_cloudwatch_dashboard" "droits_application" {
  dashboard_name = "droits-${terraform.workspace}"

  dashboard_body = jsonencode({
    widgets = [
        {
        type   = "text"
        x      = 0
        y      = 7
        width  = 3
        height = 3

        properties = {
          markdown = "Welcome to the DROITS dashboard! Here you can view metrics on low level information like CPU utilisation and the number of successful API requests made."
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
        type   = "metric"
        x      = 0
        y      = 0
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ECS",
              "HTTPCode_Target_2XX_Count",
              "ServiceName",
              "${var.ecs_backoffice_service_name}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = "eu-west-2"
          title  = "${var.ecs_backoffice_service_name} ECS service successful API requests"
        }
      },
    ]
  })
}

