variable "api_backoffice_port" {
    type = string
    description = "The port that the backoffice application runs on"
}

variable "public_subnet_1" {
    type = string
    description = "The name of the first public subnet"
}
variable "public_subnet_2" {
    type = string
    description = "The name of the second public subnet"
}

variable "api_backoffice_lb_security_group_id" {
  type = string
  description = "The ID of the security group in which the backoffice app's load balancer resides"
}

variable "webapp_port" {
    type = string
    description = "The port that the webapp runs on"
}

variable "webapp_lb_security_group_id" {
    type = string
    description = "The ID of the security group in which the webapp's load balancer resides"
}

variable "vpc_id" {
    type = string
    description = "The ID of the main vpc"
}