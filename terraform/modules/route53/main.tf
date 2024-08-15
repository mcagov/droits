resource "aws_route53_zone" "report_wreck_material" {
  name  = var.root_domain_name
}

resource "aws_route53_record" "production_webapp" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = var.root_domain_name
  type    = "A"

  alias {
    name                   = var.production_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "production_webapp_www" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = "www.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.production_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "production_webapp_subdomain" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = "webapp.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.production_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "production_backoffice" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = "backoffice.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.production_backoffice_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "staging_webapp" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = "staging.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.staging_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "staging_backoffice" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = "staging.backoffice.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.staging_backoffice_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "dev_webapp" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = "dev.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.dev_webapp_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "dev_backoffice" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = "dev.backoffice.${var.root_domain_name}"
  type    = "A"

  alias {
    name                   = var.dev_backoffice_alb_dns
    zone_id                = aws_route53_zone.report_wreck_material.zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "production_webapp_ssl_verification" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = var.production_webapp_ssl_verification_name
  type    = "CNAME"
  ttl     = 300
  records = [var.production_webapp_ssl_verification_value]
}

resource "aws_route53_record" "production_backoffice_ssl_verification" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = var.production_backoffice_ssl_verification_name
  type    = "CNAME"
  ttl     = 300
  records = [var.production_backoffice_ssl_verification_value]
}

resource "aws_route53_record" "staging_webapp_ssl_verification" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = var.staging_webapp_ssl_verification_name
  type    = "CNAME"
  ttl     = 300
  records = [var.staging_webapp_ssl_verification_value]
}

resource "aws_route53_record" "staging_backoffice_ssl_verification" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = var.staging_backoffice_ssl_verification_name
  type    = "CNAME"
  ttl     = 300
  records = [var.staging_backoffice_ssl_verification_value]
}

resource "aws_route53_record" "dev_webapp_ssl_verification" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = var.dev_webapp_ssl_verification_name
  type    = "CNAME"
  ttl     = 300
  records = [var.dev_webapp_ssl_verification_value]
}

resource "aws_route53_record" "dev_backoffice_ssl_verification" {
  zone_id = aws_route53_zone.report_wreck_material.zone_id
  name    = var.dev_backoffice_ssl_verification_name
  type    = "CNAME"
  ttl     = 300
  records = [var.dev_backoffice_ssl_verification_value]
}