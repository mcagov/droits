
data "aws_route53_zone" "droits" {
    name = "droits-route53-zone"
}

resource "aws_route53_health_check" "droits-dns-health" {
  fqdn              = "${terraform.workspace}.droits.co.uk"
  port              = 80
  type              = "HTTP"
  resource_path     = "/healthz"
  failure_threshold = "3"
  request_interval  = "10"
}

resource "aws_route53_record" "backoffice" {
  zone_id = data.aws_route53_zone.droits.zone_id
  name    = "${terraform.workspace}.droits.co.uk"
  type    = "A"
  ttl     = "60"
  health_check_id = data.aws_route53_health_check.droits-dns-health.id
  set_identifier = "ecs"

  failover_routing_policy {
    type = "PRIMARY"
  }
}