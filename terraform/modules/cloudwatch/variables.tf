variable "ecs_cluster_name" {
  type = string
}
variable "ecs_backoffice_service_name" {
  type = string
}
variable "ecs_backofice_service_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the backoffice ECS service"
}
variable "ecs_webapp_service_name" {
  type = string
}
variable "ecs_webapp_service_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the webapp ECS service"
}
variable "rds_instance_identifier" {
  type = string
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
variable "backoffice_alb_target_group_id" {
  type = string
}
variable "webapp_load_balancer" {
  type = string
}
variable "webapp_alb_id" {
  type = string
}
variable "webapp_alb_target_group_id" {
  type = string
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