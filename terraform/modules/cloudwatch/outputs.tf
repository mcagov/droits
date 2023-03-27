output "backoffice-ecs-log-group-id" {
    value = aws_cloudwatch_log_group.backoffice_ecs.id
}
output "webapp-ecs-log-group-id" {
    value = aws_cloudwatch_log_group.webapp_ecs.id
}