variable "vpc_id" {
  type = string
}

variable "private_subnets" {
  type = list(any)
}

variable "public_subnets" {
  type = list(any)
}

variable "application_name" {
  type = string
}

variable "lb_log_bucket" {
  type = string
}

variable "security_groups" {
  type = list(any)
}

variable "port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "lb_ssl_policy" {
  type        = string
  description = "Load Balancer ssl policy"
}

variable "ssl_certificate_arn" {
  type        = string
  description = "Load Balancer ssl certificate arn"
}
