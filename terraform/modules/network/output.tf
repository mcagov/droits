output "api-backoffice-id" {
  value = aws_security_group.api-backoffice.id
}

output "db-security-group-id" {
  value = aws_security_group.droits-db.id
}

output "webapp-security-group-id" {
  value = aws_security_group.webapp.id
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

output "public-subnet-1" {
  value = var.public_subnet_1
}

output "public-subnet-2" {
  value = var.public_subnet_2
}