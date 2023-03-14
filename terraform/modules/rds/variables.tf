variable "db_name" {
    type = string
    description = "Name of the DB"
    default = "droits-db"
}

variable "db_delete_protection" {
    type = bool
    description = "Should we protect the DB from being deleted?"
    default = true
}

variable "db_allocated_storage" {
    type = number
    description = "How much storage is available to the DB in GB"
    default = 50
}

locals {
  envs = { for tuple in regexall("(.*)=(.*)", file(".env")) : tuple[0] => sensitive(tuple[1]) }
}