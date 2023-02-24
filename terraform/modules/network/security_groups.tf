resource "aws_security_group" "api-backoffice" {
  name = "api-backoffice"
  vpc_id = var.aws_vpc_id
}