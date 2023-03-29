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
variable "webapp_load_balancer" {
  type = string
}