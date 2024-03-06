variable "application_name" {
  type = string
}

variable "public_subnets" {
  type = list(string)
}

variable "vpc_id" {
  type = string
}

variable "redis_port" {
  type = number
}

variable "security_groups" {
  type        = list(any)
  description = "Security groups for Elasticache"
}
