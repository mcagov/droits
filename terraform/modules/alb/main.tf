##Backoffice

resource "aws_alb_target_group" "backoffice-target-group" {
  name        = "backoffice-target-group"
  port        = var.backoffice_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.vpc.vpc_id
}

resource "aws_alb" "backoffice-alb" {
  name            = "backoffice-alb"
  subnets         = module.vpc.public_subnets
  internal        = false
  security_groups = [module.security-groups.backoffice-lb-security-group-id]
  access_logs {
    bucket  = module.s3.backoffice-lb-log-bucket
    prefix  = "backoffice_alb"
    enabled = true
  }
}


resource "aws_alb_listener" "backoffice-listener" {
  load_balancer_arn = aws_alb.backoffice-alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type = "redirect"

    redirect {
      port        = 443
      protocol    = "HTTPS"
      status_code = "HTTP_301"
    }
  }
}

resource "aws_alb_listener" "backoffice-listener-https" {
  load_balancer_arn = aws_alb.backoffice-alb.arn
  port              = 443
  protocol          = "HTTPS"

  ssl_policy      = var.lb_ssl_policy
  certificate_arn = var.ssl_certificate_arn

  default_action {
    target_group_arn = aws_alb_target_group.backoffice-target-group.arn
    type             = "forward"
  }
}

###Webapp

resource "aws_alb_target_group" "webapp-target-group" {
  name        = "webapp-target-group"
  port        = var.webapp_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.vpc.vpc_id
}

resource "aws_alb" "webapp-alb" {
  name            = "webapp-alb"
  subnets         = module.vpc.public_subnets
  internal        = false
  security_groups = [module.security-groups.webapp-lb-security-group-id]
  access_logs {
    bucket  = module.s3.webapp-lb-log-bucket
    prefix  = "webapp_alb"
    enabled = true
  }
}

resource "aws_alb_listener" "webapp-listener" {
  load_balancer_arn = aws_alb.webapp-alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type = "redirect"

    redirect {
      port        = 443
      protocol    = "HTTPS"
      status_code = "HTTP_301"
    }
  }
}

resource "aws_alb_listener" "webapp-listener-https" {
  load_balancer_arn = aws_alb.webapp-alb.arn
  port              = 443
  protocol          = "HTTPS"

  ssl_policy      = var.lb_ssl_policy
  certificate_arn = var.ssl_certificate_arn

  default_action {
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
    type             = "forward"
  }
}

