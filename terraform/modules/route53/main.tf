resource "aws_route53_zone" "report_wreck_material" {
  name = var.root_domain_name
}

resource "aws_route53_record" "dns_a_records" {
  for_each = { for record in var.a_records : record.name => record }

  name    = each.value.name
  type    = "A"
  zone_id = aws_route53_zone.report_wreck_material.zone_id

  alias {
    evaluate_target_health = false
    name                   = each.value.application == "webapp" ? var.webapp_alb_dns.dns_name : (each.value.application == "backoffice" ? var.backoffice_alb_dns.dns_name : "")
    zone_id                = each.value.application == "webapp" ? var.webapp_alb_dns.zone_id : (each.value.application == "backoffice" ? var.backoffice_alb_dns.zone_id : "")
  }
}

resource "aws_route53_record" "delegation_records" {
  for_each = var.delegated_hosted_zones

  name    = each.key
  records = each.value
  type    = "NS"
  zone_id = aws_route53_zone.report_wreck_material.zone_id
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