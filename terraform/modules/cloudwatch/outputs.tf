output "backoffice-ecs-log-group-id" {
    value = aws_cloudwatch_log_group.droits-backoffice-ecs-logs.id
}
output "webapp-ecs-log-group-id" {
    value = aws_cloudwatch_log_group.droits-webapp-ecs-logs.id
}