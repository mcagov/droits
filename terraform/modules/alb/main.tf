resource "aws_alb_target_group" "target-group" {
  name        = "${var.application_name}-target-group"
  port        = var.port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = var.vpc_id
}

resource "aws_alb" "alb" {
  name            = "${var.application_name}-alb"
  subnets         = var.public_subnets
  internal        = false
  security_groups = var.security_groups
  access_logs {
    bucket  = var.lb_log_bucket
    prefix  = "${var.application_name}_alb"
    enabled = true
  }
}


resource "aws_alb_listener" "listener" {
  load_balancer_arn = aws_alb.alb.arn
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

  depends_on = [aws_alb.alb]
}

resource "aws_alb_listener" "listener-https" {
  load_balancer_arn = aws_alb.alb.arn
  port              = 443
  protocol          = "HTTPS"

  ssl_policy      = var.lb_ssl_policy
  certificate_arn = var.ssl_certificate_arn

  default_action {
    target_group_arn = aws_alb_target_group.target-group.arn
    type             = "forward"
  }

  depends_on = [aws_alb.alb, aws_alb_target_group.target-group]
}
