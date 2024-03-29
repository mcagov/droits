resource "aws_security_group" "backoffice" {
  name   = "backoffice"
  vpc_id = var.vpc_id
  ingress {
    protocol        = "-1"
    from_port       = 0
    to_port         = 0
    security_groups = [aws_security_group.backoffice-lb.id]
    cidr_blocks     = ["0.0.0.0/0"]
    description     = "Allow inbound access from the backoffice LB only"
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow outbound access to anywhere"
  }
}

resource "aws_security_group" "backoffice-lb" {
  name   = "backoffice-lb"
  vpc_id = var.vpc_id

  ingress {
    from_port   = 80
    protocol    = "tcp"
    to_port     = 80
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow inbound HTTP access from anywhere"
  }
  ingress {
    from_port   = 443
    protocol    = "tcp"
    to_port     = 443
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow inbound HTTPS access from anywhere"
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow outbound access to anywhere"
  }
}
resource "aws_security_group" "droits-db" {
  name   = "droits-db"
  vpc_id = var.vpc_id

  ingress {
    protocol        = "tcp"
    from_port       = 5432
    to_port         = 5432
    security_groups = [aws_security_group.backoffice.id]
    description     = "Allow inbound access from the backoffice ECS service only"
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow outbound access to anywhere"
  }
}
resource "aws_security_group" "webapp" {
  name   = "webapp"
  vpc_id = var.vpc_id

  ingress {
    from_port       = 0
    to_port         = 65535
    protocol        = "tcp"
    security_groups = [aws_security_group.webapp-lb.id]
    description     = "Allow inbound access from the webapp LB only"
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow outbound access to anywhere"
  }
}

resource "aws_security_group" "webapp-lb" {
  name   = "webapp-lb"
  vpc_id = var.vpc_id
  ingress {
    from_port   = 80
    protocol    = "tcp"
    to_port     = 80
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow inbound HTTP access from anywhere"
  }
  ingress {
    from_port   = 443
    protocol    = "tcp"
    to_port     = 443
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow inbound HTTPS access from anywhere"
  }
  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
    description = "Allow outbound access to anywhere"
  }
}


resource "aws_security_group" "elasticache" {
  name        = "${var.application_name}-mca-droits-elasticache-security-group"
  description = "Allows inbound access from the task only"
  vpc_id      = var.vpc_id

  ingress {
    protocol  = "tcp"
    from_port = var.redis_port
    to_port   = var.redis_port
    #    this should allow webapp security groups
    security_groups = [aws_security_group.webapp.id]
  }

  egress {
    protocol    = "-1"
    from_port   = 0
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
  }
}
