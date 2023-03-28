resource "aws_cloudwatch_log_group" "droits-backoffice-ecs-logs" {
  name = "droits-backoffice-ecs-logs"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_stream" "backoffice" {
  name           = "droits-backoffice-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.droits-backoffice-ecs-logs.name
}


resource "aws_cloudwatch_log_group" "droits-webapp-ecs-logs" {
  name = "droits-webapp-ecs-logs"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_stream" "webapp" {
  name           = "droits-webapp-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.droits-webapp-ecs-logs.name
}

resource "aws_cloudwatch_dashboard" "droits_utilisation_and_health" {
  dashboard_name = "droits-${terraform.workspace}-utilisation-and-health"

  dashboard_body = jsonencode({
    widgets = [
        {
        type   = "text"
        x      = 0
        y      = 0
        width  = 5
        height = 3

        properties = {
          markdown = "Welcome to the DROITS dashboard! Here you can view metrics on low level information like CPU utilisation and the number of successful API requests made."
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 4
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
          title  = "${var.ecs_cluster_name} ECS cluster average CPU utilistation"
        }
      },
      {
        type   = "metric"
        x      = 13
        y      = 4
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
          stat   = "Sum"
          region = "eu-west-2"
          title  = "${var.ecs_backoffice_service_name} ECS service total successful API requests"
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
              "HTTPCode_Target_4XX_Count",
              "ServiceName",
              "${var.ecs_backoffice_service_name}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = "eu-west-2"
          title  = "${var.ecs_backoffice_service_name} ECS service total client error API requests"
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
              "HTTPCode_Target_5XX_Count",
              "ServiceName",
              "${var.ecs_backoffice_service_name}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = "eu-west-2"
          title  = "${var.ecs_backoffice_service_name} ECS service total server error API requests"
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
              "AWS/RDS",
              "CPUUtilization",
              "DBInstanceIdentifier",
              "${var.rds_instance_identifier}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = "eu-west-2"
          title  = "${var.rds_instance_identifier} average CPU utilistation"
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
              "AWS/RDS",
              "ConnectionAttempts",
              "DBInstanceIdentifier",
              "${var.rds_instance_identifier}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = "eu-west-2"
          title  = "${var.rds_instance_identifier} total number of connection attempts"
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
              "AWS/RDS",
              "CPUCreditUsage",
              "DBInstanceIdentifier",
              "${var.rds_instance_identifier}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = "eu-west-2"
          title  = "${var.rds_instance_identifier} average CPU credit usage"
        }
      }
    ]
  })
}