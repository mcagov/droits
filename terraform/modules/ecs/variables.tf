variable "ecs_cluster_name" {
  type        = string
  description = "The name of the ECS cluster that houses all our services"
}

variable "api_backoffice_port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "api-backoffice-lb-security-group-id" {
  type        = string
  description = "The ID of the security group in which the backoffice app's load balancer resides"
}

variable "webapp_port" {
  type        = string
  description = "The port that the webapp runs on"
}

variable "webapp-lb-security-group-id" {
  type        = string
  description = "The ID of the security group in which the webapp's load balancer resides"
}

