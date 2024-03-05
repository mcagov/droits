variable "application_name" {
  type = string
}

variable "public_subnets" {
  type = list(any)
}

variable "vpc_id" {
  type = number
}

variable "redis_port" {
  type = number
}

variable "security_groups" {
  type        = list(any)
  description = "Security groups for ECS"
}
