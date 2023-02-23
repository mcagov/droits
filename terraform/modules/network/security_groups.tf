resource "aws_security_group" "api-backoffice" {
  name = "api-backoffice"
  vpc_id = var.aws_vpc_id
}

output "api-backoffice-id" {
  value = aws_security_group.api-backoffice.id
}

output "vpc-id" {
  value = var.aws_vpc_id
}

output "private-subnet-1" {
  value = var.private_subnet_1
}

output "private-subnet-2" {
  value = var.private_subnet_2
}