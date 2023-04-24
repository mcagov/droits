variable "backoffice_port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "webapp_port" {
  type        = string
  description = "The port that the webapp runs on"
}

variable "vpc_id" {
  type        = string
  description = "The ID of the main vpc"
}
