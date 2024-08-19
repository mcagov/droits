variable "root_domain_name" {
  type        = string
  description = "The root domain name for DROITS"
}

variable "a_records" {
  type = list(object({
    name    = string
    alb_dns = string
  }))
  description = "List of A records for Route 53 DNS"
}

variable "ssl_certificate_arn" {
  type        = string
  description = "Load Balancer ssl certificate arn"
}

variable "domain_validation_options" {
  description = "Domain validation options for ssl certificate"
}