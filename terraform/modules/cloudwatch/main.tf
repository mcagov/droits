module "aws-rds-alarms" {
  source                               = "lorenzoaiello/rds-alarms/aws"
  version                              = "2.2.0"
  db_instance_id                       = var.db_instance_id
  db_instance_class                    = var.db_instance_class
  disk_burst_balance_too_low_threshold = var.db_low_disk_burst_balance_threshold
  # actions_alarm                        = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # actions_ok                           = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
}

module "backoffice-aws-alb-alarms" {
  source           = "lorenzoaiello/alb-alarms/aws"
  version          = "1.2.0"
  load_balancer_id = var.backoffice_alb_id
  prefix           = "elb-${var.backoffice_load_balancer}"
  target_group_id  = var.backoffice_alb_target_group_id
  # actions_alarm    = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # actions_ok       = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
}

module "webapp-aws-alb-alarms" {
  source           = "lorenzoaiello/alb-alarms/aws"
  version          = "1.2.0"
  load_balancer_id = var.webapp_alb_id
  prefix           = "elb-${var.webapp_load_balancer}"
  target_group_id  = var.backoffice_alb_target_group_id
  # actions_alarm    = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # actions_ok       = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
}

module "backoffice_ecs_service_alarms" {
  source = "cloudposse/ecs-cloudwatch-sns-alarms/aws"
  # Cloud Posse recommends pinning every module to a specific version
  version      = "0.12.3"
  namespace    = "ecs"
  stage        = terraform.workspace
  name         = "droits-${var.ecs_backoffice_service_name}"
  cluster_name = var.ecs_cluster_name
  service_name = var.ecs_backoffice_service_name
  cpu_utilization_high_alarm_actions    = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  cpu_utilization_high_ok_actions       = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  memory_utilization_high_alarm_actions = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  memory_utilization_high_ok_actions    = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
}

resource "aws_cloudwatch_metric_alarm" "backoffice_ecs_service_task_count_too_low" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.ecs_backoffice_service_name}-low-task-count"
  comparison_operator = "LessThanThreshold"
  evaluation_periods  = 1
  metric_name         = "CPUUtilization"
  namespace           = "AWS/ECS"
  period              = 60
  statistic           = "SampleCount"
  threshold           = var.ecs_backofice_service_minimum_task_count
  treat_missing_data  = "breaching"
  alarm_description   = "Task count is too low."
  # alarm_actions       = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # ok_actions          = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []

  dimensions = {
    ClusterName = var.ecs_cluster_name
    ServiceName = var.ecs_backoffice_service_name

  }
}

module "webapp_ecs_service_alarms" {
  source = "cloudposse/ecs-cloudwatch-sns-alarms/aws"
  # Cloud Posse recommends pinning every module to a specific version
  version      = "0.12.3"
  namespace    = "ecs"
  stage        = terraform.workspace
  name         = "droits-${var.ecs_webapp_service_name}"
  cluster_name = var.ecs_cluster_name
  service_name = var.ecs_webapp_service_name
  # cpu_utilization_high_alarm_actions    = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # cpu_utilization_high_ok_actions       = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # memory_utilization_high_alarm_actions = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # memory_utilization_high_ok_actions    = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
}

resource "aws_cloudwatch_metric_alarm" "webapp_ecs_service_task_count_too_low" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.ecs_webapp_service_name}-low-task-count"
  comparison_operator = "LessThanThreshold"
  evaluation_periods  = 1
  metric_name         = "CPUUtilization"
  namespace           = "AWS/ECS"
  period              = 60
  statistic           = "SampleCount"
  threshold           = var.ecs_webapp_service_minimum_task_count
  treat_missing_data  = "breaching"
  alarm_description   = "Task count is too low."
  # alarm_actions       = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []
  # ok_actions          = var.enable_alerts == true ? [aws_sns_topic.sns_technical_alerts.arn] : []

  dimensions = {
    ClusterName = var.ecs_cluster_name
    ServiceName = var.ecs_webapp_service_name

  }
}