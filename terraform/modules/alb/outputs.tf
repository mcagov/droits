output "webapp_target_group_arn" {
  value = aws_alb_target_group.webapp-target-group.arn
}

output "backoffice_target_group_arn" {
  value = aws_alb_target_group.backoffice-target-group.arn
}
