output "backoffice-id" {
  value = aws_security_group.backoffice.id
}
output "backoffice-lb-security-group-id" {
  value = aws_security_group.backoffice-lb.id
}

output "db-security-group-id" {
  value = aws_security_group.droits-db.id
}

output "webapp-security-group-id" {
  value = aws_security_group.webapp.id
}

output "webapp-lb-security-group-id" {
  value = aws_security_group.webapp-lb.id
}

output "elasticache-security-group-id" {
  value = aws_security_group.elasticache.id
}
