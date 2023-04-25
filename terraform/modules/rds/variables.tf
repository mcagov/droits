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


vpc_id                 = modules.vpc.vpc_id
vpc_security_group_ids = var.db_security_groups
subnet_ids             = var.public_subnets
