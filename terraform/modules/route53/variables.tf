variable "root_domain_name" {
  type        = string
  description = "The root domain name for DROITS"
}

variable "create_hosted_zone" {
  description = "Flag to create hosted zone"
  type        = bool
}

variable "production_webapp_alb_dns" {
  description = "The DNS name of the production webapp ALB"
  type        = string
}

variable "production_backoffice_alb_dns" {
  description = "The DNS name of the production backoffice ALB"
  type        = string
}

variable "staging_webapp_alb_dns" {
  description = "The DNS name of the staging webapp ALB"
  type        = string
}

variable "staging_backoffice_alb_dns" {
  description = "The DNS name of the staging backoffice ALB"
  type        = string
}

variable "dev_webapp_alb_dns" {
  description = "The DNS name of the dev webapp ALB"
  type        = string
}

variable "dev_backoffice_alb_dns" {
  description = "The DNS name of the dev backoffice ALB"
  type        = string
}