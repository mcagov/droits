module "aws-rds-alarms" {
  source                                          = "lorenzoaiello/rds-alarms/aws"
  version                                         = "2.2.0"
  db_instance_id                                  = var.db_instance_id
  db_instance_class                               = var.db_instance_class
  disk_burst_balance_too_low_threshold            = var.db_low_disk_burst_balance_threshold
  cpu_credit_balance_too_low_threshold            = var.db_cpu_credit_balance_too_low_threshold
  cpu_utilization_too_high_threshold              = var.db_cpu_utilization_high_threshold_percentage
  memory_freeable_too_low_threshold               = var.db_memory_freeable_too_low_threshold
  memory_swap_usage_too_high_threshold            = var.db_memory_swap_usage_too_high_threshold
  maximum_used_transaction_ids_too_high_threshold = var.db_maximum_used_transaction_ids_too_high_threshold
  evaluation_period                               = var.db_evaluation_periods
  create_anomaly_alarm                            = false
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
  actions_alarm           = var.enable_alerts == true ? [var.webapp_lb_alerts_topic_arn] : []
  actions_ok              = var.enable_alerts == true ? [var.webapp_lb_alerts_topic_arn] : []
}
module "backoffice_ecs_service_alarms" {
  source                                      = "./ecs-alarms"
  alerts_topic_arn                            = var.ecs_backoffice_alerts_topic_arn
  cluster_name                                = var.ecs_cluster_name
  enable_alerts                               = var.enable_alerts
  memory_utilization_low_threshold_percentage = var.ecs_backoffice_memory_utilization_low_threshold_percentage
  service_name                                = var.ecs_backoffice_service_name
}
module "webapp_ecs_service_alarms" {
  source                                      = "./ecs-alarms"
  alerts_topic_arn                            = var.ecs_webapp_alerts_topic_arn
  cluster_name                                = var.ecs_cluster_name
  enable_alerts                               = var.enable_alerts
  memory_utilization_low_threshold_percentage = var.ecs_webapp_memory_utilization_low_threshold_percentage
  service_name                                = var.ecs_webapp_service_name
}
