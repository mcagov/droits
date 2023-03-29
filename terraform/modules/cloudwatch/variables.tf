variable "ecs_cluster_name" {
  type = string
}
variable "ecs_backoffice_service_name" {
  type = string
}
variable "ecs_webapp_service_name" {
  type = string
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