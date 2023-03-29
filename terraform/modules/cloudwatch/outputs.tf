output "backoffice-container-log-group-id" {
  value = aws_cloudwatch_log_group.droits-backoffice-container-logs.id
}
output "webapp-container-log-group-id" {
  value = aws_cloudwatch_log_group.droits-webapp-container-logs.id
}