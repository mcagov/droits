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

variable "public_subnet_1" {
  type        = string
  description = "The ID of the public subnet in one availability zone"
}

variable "public_subnet_2" {
  type        = string
  description = "The ID of the the public subnet in another availability zone"
}


variable "db_security_group_id" {
  type        = string
  description = "The ID of the security group in which the DB resides"
}
