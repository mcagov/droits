resource "aws_security_group" "api-backoffice" {
  name = "api-backoffice"
  vpc_id = var.aws_vpc_id
}
resource "aws_security_group" "api-backoffice-lb" {
  name = "api-backoffice-lb"
  vpc_id = var.aws_vpc_id
  ingress {
    from_port         = 80
    protocol          = "tcp"
    security_group_id = aws_security_group.api-backoffice.id
    to_port           = 80
    type              = "ingress"
    source_security_group_id = aws_security_group.api-backoffice-lb.id
  }
  egress {
    from_port         = 80
    protocol          = "tcp"
    security_group_id = aws_security_group.api-backoffice.id
    to_port           = 80
    type              = "egress"
    source_security_group_id = aws_security_group.api-backoffice-lb.id
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