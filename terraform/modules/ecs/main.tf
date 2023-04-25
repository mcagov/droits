resource "aws_ecs_cluster" "droits-ecs-cluster" {
  name = var.ecs_cluster_name
}


resource "aws_ecs_task_definition" "backoffice-task-definition" {
  family                   = "backoffice"
  execution_role_arn       = var.iam_role_arn
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.backoffice_fargate_cpu
  memory                   = var.backoffice_fargate_memory
  container_definitions = jsonencode([{
    name : "backoffice",
    image : var.backoffice_image_url,
    cpu : var.backoffice_fargate_cpu,
    memory : var.backoffice_fargate_memory,
    envionment : [{ "name" : "ENV_FILE", "value" : "${var.backoffice_environment_file}" }]
    portMappings : [
      {
        containerPort : var.backoffice_port
        hostPort : var.backoffice_port
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
  # wait_for_steady_state = true


  network_configuration {
    security_groups  = var.backoffice_security_groups
    subnets          = var.public_subnets
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = var.backoffice_tg_arn
    container_name   = "backoffice"
    container_port   = var.backoffice_port
  }
}



resource "aws_ecs_task_definition" "webapp-task-definition" {
  family                   = "webapp"
  execution_role_arn       = var.iam_role_arn
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.webapp_fargate_cpu
  memory                   = var.webapp_fargate_memory
  container_definitions = jsonencode([{
    name : "webapp",
    image : var.webapp_image_url,
    cpu : var.webapp_fargate_cpu,
    memory : var.webapp_fargate_memory,
    envionment : [{ "name" : "ENV_FILE", "value" : "${var.webapp_environment_file}" }]
    portMappings : [
      {
        containerPort : var.webapp_port
        hostPort : var.webapp_port
      }
    ],
    healthCheck : {
      retries : 6,
      command : [
        "CMD-SHELL", "curl -f http://localhost:3000/health || exit 1"
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
  # wait_for_steady_state = true

  network_configuration {
    security_groups  = var.webapp_security_groups
    subnets          = var.public_subnets
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = var.webapp_tg_arn
    container_name   = "webapp"
    container_port   = var.webapp_port
  }
}
