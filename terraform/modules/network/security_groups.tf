resource "aws_security_group" "api-backoffice" {
  name = "api-backoffice"
  vpc_id = var.aws_vpc_id
}
resource "aws_security_group" "api-backoffice-lb" {
  name = "api-backoffice-lb"
  vpc_id = var.aws_vpc_id
}