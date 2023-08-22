resource "aws_ecs_task_definition" "task-definition" {
  family                   = var.service_name
  execution_role_arn       = var.iam_role_arn
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.fargate_cpu
  memory                   = var.fargate_memory
  container_definitions = jsonencode([{
    name : var.service_name,
    image : var.image_url,
    cpu : var.fargate_cpu,
    memory : var.fargate_memory,
    environment : [{ "name" : "ENV_FILE", "value" : "${var.environment_file}" }],
    portMappings : [
      {
        containerPort : var.port
        hostPort : var.port
      }
    ],
    healthCheck : {
      retries : 6,
      command : [
        "CMD-SHELL", "curl -f ${var.health_check_url} || exit 1"
      ],
    },
    logConfiguration : {
      logDriver : "awslogs",
      options : {
        awslogs-region : var.aws_region,
        awslogs-group : "droits-${var.service_name}-container-logs",
        awslogs-stream-prefix : var.service_name
      }
    }
  }])
}

resource "aws_ecs_service" "service" {
  name                              = var.service_name
  cluster                           = var.droits_ecs_cluster
  task_definition                   = aws_ecs_task_definition.task-definition.arn
  launch_type                       = "FARGATE"
  desired_count                     = var.desired_count
  health_check_grace_period_seconds = var.health_check_grace_period
  # wait_for_steady_state = true


  network_configuration {
    security_groups  = var.security_groups
    subnets          = var.public_subnets
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = var.tg_arn
    container_name   = var.service_name
    container_port   = var.port
  }
}

resource "aws_iam_role_policy_attachment" "attach_s3_access" {
  policy_arn = module.s3-images.s3_bucket_access_policy_arn
  role       = var.iam_role_name
}
