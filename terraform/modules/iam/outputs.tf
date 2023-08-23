output "iam_role_arn" {
  description = "The ARN for the IAM role that runs our ECS tasks"
  value       = aws_iam_role.ecs_task_execution.arn
}

output "iam_role_name" {
  description = "The Name for the IAM role that runs our ECS tasks"
  value       = aws_iam_role.ecs_task_execution.name
}
