variable "backoffice_port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "backoffice_lb_security_group_id" {
  type        = string
  description = "The ID of the security group in which the backoffice app's load balancer resides"
}

variable "webapp_port" {
  type        = string
  description = "The port that the webapp runs on"
}

variable "webapp_lb_security_group_id" {
  type        = string
  description = "The ID of the security group in which the webapp's load balancer resides"
}

variable "vpc_id" {
  type        = string
  description = "The ID of the main vpc"
}
