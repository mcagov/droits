output "iam-role-arn" {
  description = "The ARN for the IAM role that runs our ECS tasks"
  value       = aws_iam_role.ecs_task_execution.arn
}