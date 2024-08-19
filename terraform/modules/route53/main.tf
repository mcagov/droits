resource "aws_route53_zone" "report_wreck_material" {
  name = var.root_domain_name
}

resource "aws_route53_record" "dns_a_records" {
  for_each = { for record in var.a_records : record.name => record }
  name     = each.value.name
  type     = "A"
  ttl      = 300
  records  = [each.value.alb_dns]
  zone_id  = aws_route53_zone.report_wreck_material.zone_id

  depends_on = [aws_route53_zone.report_wreck_material]
}

resource "aws_route53_record" "dns_ssl_validation" {
  for_each = { for dvo in var.domain_validation_options : dvo.domain_name => dvo }

  name    = each.value.resource_record_name
  type    = each.value.resource_record_type
  ttl     = 60
  records = [each.value.resource_record_value]
  zone_id = aws_route53_zone.report_wreck_material.zone_id
}

resource "aws_acm_certificate_validation" "cert_validation" {
  certificate_arn         = var.ssl_certificate_arn
  validation_record_fqdns = [for record in aws_route53_record.dns_ssl_validation : record.fqdn]

  depends_on = [aws_route53_record.dns_ssl_validation]
}