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
          markdown = "Welcome to the DROITS dashboard for *${var.ecs_backoffice_service_name}*, *${var.ecs_webapp_service_name}*, *${var.rds_instance_identifier}*, *${var.backoffice_load_balancer}* and *${var.webapp_load_balancer}*! Here you can view metrics on low level information like CPU utilisation and the number of successful API requests made."
        }
      },
      {
        type   = "metric"
        x      = 4
        y      = 4
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ECS",
              "CPUUtilization",
              "ServiceName",
              "${var.ecs_backoffice_service_name}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = var.aws_region
          title  = "${var.ecs_backoffice_service_name} container service average CPU utilisation"
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 5
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ECS",
              "CPUUtilization",
              "ServiceName",
              "${var.ecs_webapp_service_name}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = var.aws_region
          title  = "${var.ecs_webapp_service_name} container service average CPU utilistation"
        }
      },
      {
        type   = "metric"
        x      = 13
        y      = 5
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
          region = var.aws_region
          title  = "${var.ecs_backoffice_service_name} container service total successful API requests"
        }
      },
      {
        type   = "metric"
        x      = 13
        y      = 11
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
          region = var.aws_region
          title  = "${var.ecs_backoffice_service_name} container service total client error API requests"
        }
      },
      {
        type   = "metric"
        x      = 13
        y      = 18
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
          region = var.aws_region
          title  = "${var.ecs_backoffice_service_name} container service total server error API requests"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 30
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
          region = var.aws_region
          title  = "${var.rds_instance_identifier} average CPU utilisation"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 36
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/RDS",
              "DatabaseConnections",
              "DBInstanceIdentifier",
              "${var.rds_instance_identifier}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.rds_instance_identifier} total number of client connections"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 42
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
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.rds_instance_identifier} total CPU credit usage"
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 48
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "ActiveConnectionCount",
              "LoadBalancer",
              "${var.backoffice_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.backoffice_load_balancer} load balancer total active connections"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 48
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "HTTPCode_ELB_4XX_Count",
              "LoadBalancer",
              "${var.backoffice_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.backoffice_load_balancer} total client error API requests originating from the load balancer"
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 54
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "HTTPCode_ELB_5XX_Count",
              "LoadBalancer",
              "${var.backoffice_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.backoffice_load_balancer} total server error API requests originating from the load balancer"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 60
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "HealthyHostCount",
              "LoadBalancer",
              "${var.backoffice_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.backoffice_load_balancer} total number of healthy targets"
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 66
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "TargetConnectionErrorCount",
              "LoadBalancer",
              "${var.backoffice_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.backoffice_load_balancer} total number of unsuccessful connections to targets"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 66
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "TargetConnectionErrorCount",
              "LoadBalancer",
              "${var.backoffice_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = var.aws_region
          title  = "${var.backoffice_load_balancer} average target response time"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 66
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "RequestCount",
              "LoadBalancer",
              "${var.backoffice_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.backoffice_load_balancer} load balancer total requests"
        }
      },
      {
        type   = "log"
        x      = 14
        y      = 66
        width  = 12
        height = 6

        properties = {
          metrics = [
              [ { "expression": "SEARCH('{AWS/ApplicationELB,LoadBalancer} MetricName=\"RequestCount\" ', 'Sum', 300)", "region": "eu-west-2" } ]
          ],
          legend = {
              position = "bottom"
          },
          title  = "${var.backoffice_load_balancer} and ${var.webapp_load_balancer} load balancer total requests",
          region = "eu-west-2",
          liveData = false,
          view = "timeSeries",
          stacked = false
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 72
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "ActiveConnectionCount",
              "LoadBalancer",
              "${var.webapp_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.webapp_load_balancer} load balancer total active connections"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 72
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "HTTPCode_ELB_4XX_Count",
              "LoadBalancer",
              "${var.webapp_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.webapp_load_balancer} total client error API requests originating from the load balancer"
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 78
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "HTTPCode_ELB_5XX_Count",
              "LoadBalancer",
              "${var.webapp_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.webapp_load_balancer} total server error API requests originating from the load balancer"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 78
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "HealthyHostCount",
              "LoadBalancer",
              "${var.webapp_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.webapp_load_balancer} total number of healthy targets"
        }
      },
      {
        type   = "metric"
        x      = 0
        y      = 84
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "TargetConnectionErrorCount",
              "LoadBalancer",
              "${var.webapp_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Sum"
          region = var.aws_region
          title  = "${var.webapp_load_balancer} total number of unsuccessful connections to targets"
        }
      },
      {
        type   = "metric"
        x      = 14
        y      = 84
        width  = 12
        height = 6

        properties = {
          metrics = [
            [
              "AWS/ApplicationELB",
              "TargetConnectionErrorCount",
              "TargetResponseTime",
              "${var.webapp_load_balancer}"
            ]
          ]
          period = 300
          stat   = "Average"
          region = var.aws_region
          title  = "${var.webapp_load_balancer} average target response time"
        }
      }
    ]
  })
}