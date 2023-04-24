variable "ecs_cluster_name" {
  type        = string
  description = "The name of the ECS cluster that houses all our services"
}

variable "backoffice_port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "webapp_port" {
  type        = string
  description = "The port that the webapp runs on"
}
