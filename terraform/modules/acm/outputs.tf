output "ssl_certificate_arn" {
  description = "ARN of ssl certificate"
  value       = aws_acm_certificate.ssl_cert.arn
}

output "domain_validation_options" {
  description = "Domain validation options for ssl certificate"
  value       = aws_acm_certificate.ssl_cert.domain_validation_options
}