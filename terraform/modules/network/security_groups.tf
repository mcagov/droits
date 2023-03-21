resource "aws_security_group" "api-backoffice" {
  name = "api-backoffice"
  vpc_id = var.aws_vpc_id
  ingress {
    from_port         = 0
    protocol          = "-1"
#    security_groups   = [aws_security_group.api-backoffice-lb.id]
    cidr_blocks = ["0.0.0.0/0"]
    to_port           = 0
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
  }
}
resource "aws_security_group" "api-backoffice-lb" {
  name = "api-backoffice-lb"
  vpc_id = var.aws_vpc_id
  ingress {
    from_port         = 80
    protocol          = "tcp"
    to_port           = 80
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    from_port         = 443
    protocol          = "tcp"
    to_port           = 443
    cidr_blocks = ["0.0.0.0/0"]
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
  }
}
resource "aws_security_group" "droits-db" {
  name = "droits-db"
  vpc_id = var.aws_vpc_id
}
resource "aws_security_group" "webapp" {
  name = "webapp"
  vpc_id = var.aws_vpc_id

  ingress {
    from_port         = 0
    protocol          = "-1"
    cidr_blocks = ["0.0.0.0/0"]
    to_port           = 0
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
  }
}