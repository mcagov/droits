resource "aws_route53_zone" "report_wreck_material" {
  name = var.root_domain_name
  count = var.create_hosted_zone ? 1 : 0
}

resource "aws_route53_record" "production_webapp" {
  count  = var.create_hosted_zone ? 1 : 0
  zone_id = aws_route53_zone.report_wreck_material[0].zone_id
  name    = var.root_domain_name
  type    = "A"

  alias {
    name                   = var.production_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material[0].zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "production_backoffice" {
  count  = var.create_hosted_zone ? 1 : 0
  zone_id = aws_route53_zone.report_wreck_material[0].zone_id
  name    = "backoffice.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.production_backoffice_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material[0].zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "staging_webapp" {
  count  = var.create_hosted_zone ? 1 : 0
  zone_id = aws_route53_zone.report_wreck_material[0].zone_id
  name    = "staging.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.staging_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material[0].zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "staging_backoffice" {
  count  = var.create_hosted_zone ? 1 : 0
  zone_id = aws_route53_zone.report_wreck_material[0].zone_id
  name    = "staging.backoffice.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.staging_backoffice_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material[0].zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "dev_webapp" {
  count  = var.create_hosted_zone ? 1 : 0
  zone_id = aws_route53_zone.report_wreck_material[0].zone_id
  name    = "dev.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.dev_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material[0].zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "dev_backoffice" {
  count  = var.create_hosted_zone ? 1 : 0
  zone_id = aws_route53_zone.report_wreck_material[0].zone_id
  name    = "dev.backoffice.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.dev_backoffice_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material[0].zone_id
    evaluate_target_health = false
  }
}