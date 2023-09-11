data "aws_iam_policy_document" "ecs_task_execution_agent" {
  version = "2012-10-17"
  statement {
    sid     = ""
    effect  = "Allow"
    actions = ["sts:AssumeRole"]

    principals {
      type        = "Service"
      identifiers = ["ecs-tasks.amazonaws.com"]
    }
  }
}

resource "aws_iam_role" "ecs_task_execution" {
  name               = "ecs-${terraform.workspace}-execution-role"
  assume_role_policy = data.aws_iam_policy_document.ecs_task_execution_agent.json
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_rules" {
  role       = aws_iam_role.ecs_task_execution.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_iam_policy" "ecs_instance_metadata" {
  name        = "ECSInstanceMetadataAccess"
  description = "Allow access to EC2 instance metadata"

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Action = "ec2:Metadata",
        Resource = "*"
      }
    ]
  })
}

resource "aws_iam_policy_attachment" "ecs_instance_metadata_attachment" {
  name       = "ECSTaskExecutionMetadataAttachment"
  policy_arn = aws_iam_policy.ecs_instance_metadata.arn
  roles      = [aws_iam_role.ecs_task_execution.name]
}
