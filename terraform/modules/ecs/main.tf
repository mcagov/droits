resource "aws_ecs_cluster" "droits-ecs-cluster" {
  name         = var.ecs_cluster_name
}

resource "aws_alb_target_group" "api-backoffice-target-group" {
  name        = "api-backoffice-target-group"
  port        = var.api_backoffice_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = var.vpc_id
}

resource "aws_alb" "api-backoffice-alb" {
  name         = "api-backoffice-alb"
  subnets      = [var.public_subnet_1,var.public_subnet_2]
  internal     = false
  security_groups = [ var.api_backoffice_lb_security_group_id ]
}

resource "aws_alb_listener" "api-backoffice-listener" {
  load_balancer_arn = aws_alb.api-backoffice-alb.arn
  default_action {
    type = "forward"
    target_group_arn = aws_alb_target_group.api-backoffice-target-group.arn
  }
  port              = 80
}

resource "aws_alb_target_group" "webapp-target-group" {
  name        = "webapp-target-group"
  port        = var.webapp_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = var.vpc_id
}

resource "aws_alb" "webapp-alb" {
  name         = "webapp-alb"
  subnets      = [var.public_subnet_1,var.public_subnet_2]
  internal     = false
  security_groups = [ var.webapp_lb_security_group_id ]
}

resource "aws_alb_listener" "webapp-listener" {
  load_balancer_arn = aws_alb.webapp-alb.arn
  default_action {
    type = "forward"
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
  }
  port              = 80
}

resource "aws_ecs_task_definition" "backoffice-task-definition" {
  family                    = "backoffice"
  execution_role_arn        = var.execution_role_arn
  requires_compatibilities  = ["FARGATE"]
  network_mode              = "awsvpc"
  cpu                       = var.backoffice_fargate_cpu
  memory                    = var.backoffice_fargate_memory
  container_definitions     = jsonencode([{
    name : "api-backoffice",
    image : var.backoffice_image_url,
    portMappings : [
      {
        containerPort : var.api_backoffice_port
        hostPort : var.api_backoffice_port
      }
    ],
    healthCheck : {
      retries : 6,
      command : [
        "CMD-SHELL", "curl -f http://localhost:5000/health || exit 1"
      ],
    }
  }])
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture = "ARM64"
  }
}

resource "aws_ecs_service" "backoffice-service" {
  name            = "backoffice"
  cluster         = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition = aws_ecs_task_definition.backoffice-task-definition.arn
  launch_type     = "FARGATE"
  desired_count   = 1
  health_check_grace_period_seconds = 600
  # wait_for_steady_state = true
  depends_on = [
    aws_alb_listener.api-backoffice-listener,
  ]

  network_configuration {
    security_groups   = [var.backoffice_security_group]
    subnets           = [var.public_subnet_1]
    assign_public_ip  = true
  }
  
  load_balancer {
    target_group_arn  = aws_alb_target_group.api-backoffice-target-group.arn
    container_name    = "api-backoffice"
    container_port    = var.api_backoffice_port
  }
}

resource "aws_ecs_task_definition" "webapp-task-definition" {
  family                = "webapp"
  execution_role_arn = var.execution_role_arn
  requires_compatibilities  = ["FARGATE"]
  network_mode              = "awsvpc"
  cpu                       = var.webapp_fargate_cpu
  memory                    = var.webapp_fargate_memory
  container_definitions = jsonencode([{
    name : "webapp",
    image : var.webapp_image_url,
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
    }
  }])
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture = "ARM64"
  }
}

resource "aws_ecs_service" "webapp" {
  name = "webapp"
  cluster = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition = aws_ecs_task_definition.webapp-task-definition.arn
  launch_type = "FARGATE"
  desired_count = 1
  health_check_grace_period_seconds = 600
  # wait_for_steady_state = true
  depends_on = [
    aws_alb_listener.webapp-listener,
  ]
  
  network_configuration {
    security_groups = [var.webapp_security_group]
    subnets         = [var.public_subnet_1]
    assign_public_ip = true
  }
  
  load_balancer {
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
    container_name = "webapp"
    container_port = var.webapp_port
  }
}