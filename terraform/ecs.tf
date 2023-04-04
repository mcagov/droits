resource "aws_ecs_cluster" "droits-ecs-cluster" {
  name = var.ecs_cluster_name
}

resource "aws_ecs_task_definition" "backoffice-task-definition" {
  family                   = "backoffice"
  execution_role_arn       = module.iam.iam-role-arn
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.api_backoffice_fargate_cpu
  memory                   = var.api_backoffice_fargate_memory
  container_definitions = jsonencode([{
    name : "api-backoffice",
    image : "${var.ecr_repository_url}/${var.api_backoffice_ecr_repository_name}:${var.image_tag}",
    portMappings : [
      {
        containerPort : var.api_backoffice_port
        hostPort : var.api_backoffice_port
      }
    ],
    healthCheck : {
      retries : 6,
      command : [
        "CMD-SHELL", "curl -f http://localhost:5000/healthz || exit 1"
      ],
    },
    logConfiguration : {
      logDriver : "awslogs",
      options : {
        awslogs-region : var.aws_region,
        awslogs-group : "droits-backoffice-container-logs",
        awslogs-stream-prefix : "backoffice"
      }
    }
  }])
}

resource "aws_ecs_service" "backoffice-service" {
  name                              = "backoffice"
  cluster                           = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition                   = aws_ecs_task_definition.backoffice-task-definition.arn
  launch_type                       = "FARGATE"
  desired_count                     = 1
  health_check_grace_period_seconds = 600
  force_new_deployment              = true

  depends_on = [
    aws_alb_listener.api-backoffice-listener,
    aws_alb_listener.api-backoffice-listener-https
  ]

  network_configuration {
    security_groups  = [module.security-groups.api-backoffice-id]
    subnets          = [module.security-groups.public-subnet-1]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_alb_target_group.api-backoffice-target-group.arn
    container_name   = "api-backoffice"
    container_port   = var.api_backoffice_port
  }
}

resource "aws_ecs_task_definition" "webapp-task-definition" {
  family                   = "webapp"
  execution_role_arn       = module.iam.iam-role-arn
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.webapp_fargate_cpu
  memory                   = var.webapp_fargate_memory

  container_definitions = jsonencode([{
    name : "webapp",
    image : "${var.ecr_repository_url}/${var.webapp_ecr_repository_name}:${var.image_tag}",
    portMappings : [
      {
        containerPort : var.webapp_port
        hostPort : var.webapp_port
      }
    ],
    healthCheck : {
      retries : 6,
      command : [
        "CMD-SHELL", "curl -f http://localhost:5000/health || exit 1"
      ],
    },
    logConfiguration : {
      logDriver : "awslogs",
      options : {
        awslogs-region : var.aws_region,
        awslogs-group : "droits-webapp-container-logs",
        awslogs-stream-prefix : "webapp"
      }
    }
  }])
}

resource "aws_ecs_service" "webapp" {
  name                              = "webapp"
  cluster                           = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition                   = aws_ecs_task_definition.webapp-task-definition.arn
  launch_type                       = "FARGATE"
  desired_count                     = 1
  health_check_grace_period_seconds = 600
  force_new_deployment              = true

  depends_on = [
    aws_alb_listener.webapp-listener,
    aws_alb_listener.webapp-listener-https
  ]

  network_configuration {
    security_groups  = [module.security-groups.webapp-security-group-id]
    subnets          = [module.security-groups.public-subnet-1]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
    container_name   = "webapp"
    container_port   = var.webapp_port
  }
}
