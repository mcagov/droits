resource "aws_acm_certificate" "ssl_cert" {
  domain_name               = var.ssl_domains[0]
  subject_alternative_names = slice(var.ssl_domains, 1, length(var.ssl_domains))
  validation_method         = "DNS"

  lifecycle {
    create_before_destroy = true
  }

  tags = {
    Name = "DroitsACMCertificate"
  }
}

resource "aws_acm_certificate" "imported_certificate" {
  domain_name = "*.report-wreck-material.service.gov.uk"
  subject_alternative_names = [
    "*.backoffice.report-wreck-material.service.gov.uk",
    "report-wreck-material.service.gov.uk"
  ]
}