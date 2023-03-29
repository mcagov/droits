##Backoffice

resource "aws_alb_target_group" "api-backoffice-target-group" {
  name        = "api-backoffice-target-group"
  port        = var.api_backoffice_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.security-groups.vpc-id
}

resource "aws_alb" "api-backoffice-alb" {
  name            = "api-backoffice-alb"
  subnets         = [module.security-groups.public-subnet-1, module.security-groups.public-subnet-2]
  internal        = false
  security_groups = [module.security-groups.api-backoffice-lb-security-group-id]
  access_logs {
    bucket  = module.s3.backoffice-lb-log-bucket
    prefix  = "backoffice_alb"
    enabled = true
  }
}


resource "aws_alb_listener" "api-backoffice-listener" {
  load_balancer_arn = aws_alb.api-backoffice-alb.arn
  port              = 80
  protocol          = "HTTP"

  #  default_action {
  #    type = "redirect"
  #
  #    redirect {
  #      port        = 443
  #      protocol    = "HTTPS"
  #      status_code = "HTTP_301"
  #    }
  #  }
  default_action {
    target_group_arn = aws_alb_target_group.api-backoffice-target-group.arn
    type             = "forward"
  }
}

#resource "aws_alb_listener" "api-backoffice-listener-https" {
#  load_balancer_arn = aws_alb.api-backoffice-alb.arn
#  port              = 443
#  protocol          = "HTTPS"
#
#  ssl_policy      = var.lb_ssl_policy
#  certificate_arn = var.ssl_certificate_arn
#
#  default_action {
#    target_group_arn = aws_alb_target_group.api-backoffice-target-group.arn
#    type             = "forward"
#  }
#}

###Webapp

resource "aws_alb_target_group" "webapp-target-group" {
  name        = "webapp-target-group"
  port        = var.webapp_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.security-groups.vpc-id
}

resource "aws_alb" "webapp-alb" {
  name            = "webapp-alb"
  subnets         = [module.security-groups.public-subnet-1, module.security-groups.public-subnet-2]
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

  #  default_action {
  #    type = "redirect"
  #
  #    redirect {
  #      port        = 443
  #      protocol    = "HTTPS"
  #      status_code = "HTTP_301"
  #    }
  #  }
  default_action {
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
    type             = "forward"
  }
}

#resource "aws_alb_listener" "webapp-listener-https" {
#  load_balancer_arn = aws_alb.webapp-alb.arn
#  port              = 443
#  protocol          = "HTTPS"
#
#  ssl_policy      = var.lb_ssl_policy
#  certificate_arn = var.ssl_certificate_arn
#
#  default_action {
#    target_group_arn = aws_alb_target_group.webapp-target-group.arn
#    type             = "forward"
#  }
#}

