data "aws_iam_policy_document" "ecs_task_execution_agent" {
  version = "2012-10-17"

  statement {
    sid    = ""
    effect = "Allow"
    actions = [
      "sts:AssumeRole"
    ]

    principals {
      type = "Service"
      identifiers = [
        "ecs-tasks.amazonaws.com",
        "ec2.amazonaws.com",
        "s3.amazonaws.com"
      ]
    }
  }
}

resource "aws_iam_role" "ecs_task_execution_role" {
  name               = "ecs-${terraform.workspace}-execution-role"
  assume_role_policy = data.aws_iam_policy_document.ecs_task_execution_agent.json
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_rules" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

resource "aws_iam_role" "ecs_task_role" {
  name               = "ecs-${terraform.workspace}-task-role"
  assume_role_policy = data.aws_iam_policy_document.ecs_task_execution_agent.json
}

resource "aws_iam_policy" "ecs_task_role_policy" {
  name        = "ecs_task_role_policy"
  description = "EC2 policy for ECS task role"
  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Action = [
          "ec2:DescribeInstances"
        ],
        Resource = "*"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "ecs_task_role_policy_attachment" {
  policy_arn = aws_iam_policy.ecs_task_role_policy.arn
  role       = aws_iam_role.ecs_task_role.name
}
