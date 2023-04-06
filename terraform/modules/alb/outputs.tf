output "webapp-target-group-arn" {
  value = aws_alb_target_group.webapp-target-group
}

output "api-backoffice-target-group-arn" {
  value = aws_alb_target_group.api-backoffice-target-group
}