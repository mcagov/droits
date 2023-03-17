resource "aws_security_group" "api-backoffice" {
  name = "api-backoffice"
  vpc_id = var.aws_vpc_id
  ingress {
    from_port         = 80
    protocol          = "tcp"
    security_groups   = [aws_security_group.api-backoffice-lb.id]
    to_port           = 80
  }
  egress {
    from_port         = 80
    protocol          = "tcp"
    to_port           = 80
  }
}
resource "aws_security_group" "api-backoffice-lb" {
  name = "api-backoffice-lb"
  vpc_id = var.aws_vpc_id
  ingress {
    from_port         = 80
    protocol          = "tcp"
    to_port           = 80
  }
  egress {
    from_port         = 80
    protocol          = "tcp"
    to_port           = 80
  }
}
resource "aws_security_group" "droits-db" {
  name = "droits-db"
  vpc_id = var.aws_vpc_id
}
resource "aws_security_group" "webapp" {
  name = "webapp"
  vpc_id = var.aws_vpc_id
}