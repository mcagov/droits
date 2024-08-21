variable "root_domain_name" {
  type        = string
  description = "The root domain name for DROITS"
}

variable "webapp_alb_dns" {
  type = object({
    zone_id  = string
    dns_name = string
  })
  description = "DNS information for webapp ALB"
}

variable "backoffice_alb_dns" {
  type = object({
    zone_id  = string
    dns_name = string
  })
  description = "DNS information for backoffice ALB"
}

variable "a_records" {
  type = list(object({
    name        = string
    application = string
  }))
  description = "List of A records for Route 53 DNS"
}

variable "ssl_certificate_arn" {
  type        = string
  description = "ACM SSL certificate arn"
}

variable "domain_validation_options" {
  description = "Domain validation options for ssl certificate"
}