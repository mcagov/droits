output "iam_execution_role_arn" {
  description = "The ARN for the IAM execution role that runs our ECS tasks"
  value       = aws_iam_role.ecs_task_execution_role.arn
}

output "iam_execution_role_name" {
  description = "The Name for the IAM execution role that runs our ECS tasks"
  value       = aws_iam_role.ecs_task_execution_role.name
}


output "iam_task_role_arn" {
  description = "The ARN for the IAM task role that runs in our ECS tasks"
  value       = aws_iam_role.ecs_task_role.arn
}

output "iam_task_role_name" {
  description = "The Name for the IAM task role that runs in our ECS tasks"
  value       = aws_iam_role.ecs_task_role.name
}
