resource "aws_alb_target_group" "api-backoffice-target-group" {
  name        = "api-backoffice-target-group"
  port        = var.api_backoffice_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = var.vpc_id
}

resource "aws_alb" "api-backoffice-alb" {
  name         = "api-backoffice-alb"
  subnets      = [var.public_subnet_1,var.public_subnet_2]
  internal     = false
  security_groups = [ var.api_backoffice_lb_security_group_id ]
}

resource "aws_alb_listener" "api-backoffice-listener" {
  load_balancer_arn = aws_alb.api-backoffice-alb.arn
  default_action {
    type = "forward"
    target_group_arn = aws_alb_target_group.api-backoffice-target-group.arn
  }
  port              = 80
}

resource "aws_alb_target_group" "webapp-target-group" {
  name        = "webapp-target-group"
  port        = var.webapp_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = var.vpc_id
}

resource "aws_alb" "webapp-alb" {
  name         = "webapp-alb"
  subnets      = [var.public_subnet_1,var.public_subnet_2]
  internal     = false
  security_groups = [ var.webapp_lb_security_group_id ]
}

resource "aws_alb_listener" "webapp-listener" {
  load_balancer_arn = aws_alb.webapp-alb.arn
  default_action {
    type = "forward"
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
  }
  port              = 80
}