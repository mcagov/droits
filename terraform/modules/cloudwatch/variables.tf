variable "ecs_cluster_name" {
  type = string
}
variable "ecs_backoffice_service_name" {
  type = string
}
variable "ecs_backoffice_alerts_topic_arn" {
  type        = string
  description = "The ARN of the backoffice_alerts SNS topic"
}
variable "ecs_webapp_service_name" {
  type = string
}
variable "ecs_webapp_service_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the webapp ECS service"
}
variable "ecs_webapp_alerts_topic_arn" {
  type        = string
  description = "The ARN of the webapp_alerts SNS topic"
}
variable "rds_instance_identifier" {
  type = string
}
variable "rds_db_alerts_topic_arn" {
  type        = string
  description = "The ARN of the db_alerts SNS topic"
}
variable "aws_region" {
  type = string
}
variable "backoffice_load_balancer" {
  type = string
}
variable "backoffice_alb_id" {
  type = string
}
variable "backoffice_alb_arn_suffix" {
  type        = string
  description = "The final portion of the backoffice load balancer's ARN"
}
variable "backoffice_alb_target_group_id" {
  type = string
}
variable "backoffice_lb_alerts_topic_arn" {
  type        = string
  description = "The ARN of the backoffice_lb_alerts SNS topic"
}
variable "ecs_backoffice_memory_utilization_low_threshold_percentage" {
  type        = number
  description = "The % CPU utilisation limit which, when consistently under, will trigger an alarm."
  default     = 6
}
variable "ecs_webapp_memory_utilization_low_threshold_percentage" {
  type        = number
  description = "The % CPU utilisation limit which, when consistently under, will trigger an alarm."
  default     = 8
}
variable "webapp_load_balancer" {
  type = string
}
variable "webapp_alb_id" {
  type = string
}
variable "webapp_alb_arn_suffix" {
  type        = string
  description = "The final portion of the webapp load balancer's ARN"
}
variable "webapp_alb_target_group_id" {
  type = string
}
variable "webapp_lb_alerts_topic_arn" {
  type        = string
  description = "The ARN of the webapp_lb_alerts SNS topic"
}
variable "db_instance_id" {
  type = string
}
variable "db_instance_class" {
  type = string
}
variable "db_low_disk_burst_balance_threshold" {
  type = number
}
variable "enable_alerts" {
  type        = bool
  description = "When enabled CloudWatch alarm events are sent to the Alerts SNS Topic"
}
variable "db_cpu_utilization_high_threshold_percentage" {
  type        = number
  description = "The % CPU utilisation limit which, when passed, will trigger an alarm. This will be higher for dev and lower for production."
  default     = 90
}
variable "db_cpu_credit_balance_too_low_threshold" {
  type        = string
  description = "Threshold for the DB credit balance too low alarm"
}
variable "db_evaluation_periods" {
  type        = string
  description = "The number of periods to evaluate for the alarm"
}
variable "db_memory_freeable_too_low_threshold" {
  type        = string
  description = "Threshold for the DB freeable memory too low alarm"
}
variable "db_memory_swap_usage_too_high_threshold" {
  type        = string
  description = "Threshold for the DB memory swap usage too high alarm"
}
variable "db_maximum_used_transaction_ids_too_high_threshold" {
  type        = string
  description = "Threshold for the maximum used transaction IDs DB alarm"
}
variable "lb_average_response_time_threshold" {
  type        = string
  description = "Maximum length of time for a host to respond before triggering the alarm"
}
variable "lb_evaluation_periods" {
  type        = string
  description = "The number of periods to evaluate for the alarm"
}