module "aws-rds-alarms" {
  source                                          = "lorenzoaiello/rds-alarms/aws"
  version                                         = "2.2.0"
  db_instance_id                                  = var.db_instance_id
  db_instance_class                               = var.db_instance_class
  disk_burst_balance_too_low_threshold            = var.db_low_disk_burst_balance_threshold
  cpu_credit_balance_too_low_threshold            = var.db_cpu_credit_balance_too_low_threshold
  cpu_utilization_too_high_threshold              = var.percentage_cpu_utilization_high_threshold
  memory_freeable_too_low_threshold               = var.db_memory_freeable_too_low_threshold
  memory_swap_usage_too_high_threshold            = var.db_memory_swap_usage_too_high_threshold
  maximum_used_transaction_ids_too_high_threshold = var.db_maximum_used_transaction_ids_too_high_threshold
  evaluation_period                               = var.db_evaluation_periods
  actions_alarm                                   = var.enable_alerts == true ? [var.rds_db_alerts_topic_arn] : []
  actions_ok                                      = var.enable_alerts == true ? [var.rds_db_alerts_topic_arn] : []
}

module "backoffice-aws-alb-alarms" {
  source                  = "./pagerduty-alarms"
  load_balancer_id        = var.backoffice_alb_id
  prefix                  = "elb-${var.backoffice_load_balancer}"
  target_group_id         = var.backoffice_alb_target_group_id
  response_time_threshold = var.lb_average_response_time_threshold
  evaluation_period       = var.lb_evaluation_periods
  actions_alarm           = var.enable_alerts == true ? [var.backoffice_lb_alerts_topic_arn] : []
  actions_ok              = var.enable_alerts == true ? [var.backoffice_lb_alerts_topic_arn] : []
}

module "webapp-aws-alb-alarms" {
  source                  = "./pagerduty-alarms"
  load_balancer_id        = var.webapp_alb_id
  prefix                  = "elb-${var.webapp_load_balancer}"
  target_group_id         = var.webapp_alb_target_group_id
  response_time_threshold = var.lb_average_response_time_threshold
  evaluation_period       = var.lb_evaluation_periods
  actions_alarm           = [var.webapp_lb_alerts_topic_arn]
  actions_ok              = [var.webapp_lb_alerts_topic_arn]
}

module "backoffice_ecs_service_alarms" {
  source = "cloudposse/ecs-cloudwatch-sns-alarms/aws"
  # Cloud Posse recommends pinning every module to a specific version
  version                                    = "0.12.3"
  namespace                                  = "ecs"
  stage                                      = terraform.workspace
  name                                       = "droits-${var.ecs_backoffice_service_name}"
  cluster_name                               = var.ecs_cluster_name
  service_name                               = var.ecs_backoffice_service_name
  cpu_utilization_high_threshold             = var.percentage_cpu_utilization_high_threshold
  cpu_utilization_high_alarm_actions         = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  cpu_utilization_high_ok_actions            = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  memory_utilization_high_threshold          = var.percentage_memory_utilization_high_threshold
  memory_utilization_high_evaluation_periods = var.memory_utilization_high_evaluation_periods
  memory_utilization_high_period             = var.memory_utilisation_duration_in_seconds_to_evaluate
  memory_utilization_high_alarm_actions      = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  memory_utilization_high_ok_actions         = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
}

resource "aws_cloudwatch_metric_alarm" "backoffice_ecs_service_task_count_too_low" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.ecs_backoffice_service_name}-low-task-count"
  comparison_operator = "LessThanThreshold"
  evaluation_periods  = var.cpu_utilization_high_evaluation_periods
  metric_name         = "CPUUtilization"
  namespace           = "AWS/ECS"
  period              = var.cpu_utilisation_duration_in_seconds_to_evaluate
  statistic           = "SampleCount"
  threshold           = var.ecs_backofice_service_minimum_task_count
  treat_missing_data  = "breaching"
  alarm_description   = "Task count is too low."
  alarm_actions       = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  ok_actions          = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []

  dimensions = {
    ClusterName = var.ecs_cluster_name
    ServiceName = var.ecs_backoffice_service_name
  }
}

module "webapp_ecs_service_alarms" {
  source                                     = "cloudposse/ecs-cloudwatch-sns-alarms/aws"
  version                                    = "0.12.3"
  namespace                                  = "ecs"
  stage                                      = terraform.workspace
  name                                       = "droits-${var.ecs_webapp_service_name}"
  cluster_name                               = var.ecs_cluster_name
  service_name                               = var.ecs_webapp_service_name
  cpu_utilization_high_threshold             = var.percentage_cpu_utilization_high_threshold
  cpu_utilization_high_alarm_actions         = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  cpu_utilization_high_ok_actions            = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  memory_utilization_high_threshold          = var.percentage_memory_utilization_high_threshold
  memory_utilization_high_evaluation_periods = var.memory_utilization_high_evaluation_periods
  memory_utilization_high_period             = var.memory_utilisation_duration_in_seconds_to_evaluate
  memory_utilization_high_alarm_actions      = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
  memory_utilization_high_ok_actions         = var.enable_alerts == true ? [var.ecs_backoffice_alerts_topic_arn] : []
}

resource "aws_cloudwatch_metric_alarm" "webapp_ecs_service_task_count_too_low" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.ecs_webapp_service_name}-low-task-count"
  comparison_operator = "LessThanThreshold"
  evaluation_periods  = var.cpu_utilization_high_evaluation_periods
  metric_name         = "CPUUtilization"
  namespace           = "AWS/ECS"
  period              = var.cpu_utilisation_duration_in_seconds_to_evaluate
  statistic           = "SampleCount"
  threshold           = var.ecs_webapp_service_minimum_task_count
  treat_missing_data  = "breaching"
  alarm_description   = "Task count is too low."
  alarm_actions       = var.enable_alerts == true ? [var.ecs_webapp_alerts_topic_arn] : []
  ok_actions          = var.enable_alerts == true ? [var.ecs_webapp_alerts_topic_arn] : []

  dimensions = {
    ClusterName = var.ecs_cluster_name
    ServiceName = var.ecs_webapp_service_name
  }
}
