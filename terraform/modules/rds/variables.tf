variable "db_name" {
  type        = string
  description = "Name of the DB"
  default     = "droits"
}

variable "db_username" {
  type        = string
  description = "The username for the master database user"
  default     = "droits"
  sensitive   = true
}
variable "db_password" {
  type        = string
  description = "The password used for the master database user"
  sensitive   = true
}

variable "db_instance_class" {
  type        = string
  description = "The database instance class"
}

variable "db_delete_protection" {
  type        = bool
  description = "Should we protect the DB from being deleted?"
}

variable "db_allocated_storage" {
  type        = number
  description = "How much storage is available to the DB in GB"
  default     = 50
}

variable "vpc_id" {
  type = string
}

variable "private_subnets" {
  type = list(any)
}

variable "public_subnets" {
  type = list(any)
}

variable "db_security_groups" {
  type = list(any)
}
