variable "vpc_id" {
  type = string
}

variable "private_subnets" {
  type = list(any)
}

variable "public_subnets" {
  type = list(any)
}


variable "backoffice_lb_log_bucket" {
  type = string
}

variable "backoffice_security_groups" {
  type = list(any)
}

variable "webapp_lb_log_bucket" {
  type = string
}

variable "webapp_security_groups" {
  type = list(any)
}

variable "backoffice_port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "webapp_port" {
  type        = string
  description = "The port that the webapp runs on"
}
