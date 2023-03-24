terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.27.0"
    }
  }

  backend "s3" {
    bucket  = "droits-statefile"
    key     = "global/s3/terraform.tfstate"
    encrypt = true
    region  = "eu-west-2"
    profile = "droits_dev"
  }
}

provider "aws" {
  region     = var.aws_region
  access_key = var.aws_access_key_id
  secret_key = var.aws_secret_access_key
}

module "security-groups" {
  aws_vpc_id       = var.aws_vpc_id
  private_subnet_1 = var.private_subnet_1
  private_subnet_2 = var.private_subnet_2
  public_subnet_1  = var.public_subnet_1
  public_subnet_2  = var.public_subnet_2
  source           = "./modules/security-groups"
}

module "iam" {
  source = "./modules/iam"
}

module "rds" {
  public_subnet_1      = module.security-groups.public-subnet-1
  public_subnet_2      = module.security-groups.public-subnet-2
  db_security_group_id = module.security-groups.db-security-group-id
  db_delete_protection = var.db_delete_protection
  source               = "./modules/rds"
  db_password          = var.db_password
  db_username          = var.db_username
}

# module "ecs" {

# }

resource "aws_s3_bucket" "droits-wreck-images" {
  bucket = "droits-wreck-images"
  # Stops terraform from destroying the object if it exists
  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_s3_bucket_acl" "droits-wreck-images-acl" {
  bucket = "droits-wreck-images"
  acl    = "private"
}

resource "aws_s3_bucket_versioning" "droits-wreck-images-versioning" {
  bucket = "droits-wreck-images"
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "droits-wreck-images-encryption-config" {
  bucket = "droits-wreck-images"
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_ecr_repository" "droits-webapp-repository" {
  name         = var.webapp_ecr_repository_name
  force_delete = true
}

resource "aws_ecr_repository" "droits-api-backoffice-repository" {
  name         = var.api_backoffice_ecr_repository_name
  force_delete = true
}

resource "aws_ecs_cluster" "droits-ecs-cluster" {
  name = var.ecs_cluster_name
}

resource "aws_alb_target_group" "api-backoffice-target-group" {
  name        = "api-backoffice-target-group"
  port        = var.api_backoffice_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.security-groups.vpc-id
}

resource "aws_alb" "api-backoffice-alb" {
  name            = "api-backoffice-alb"
  subnets         = [module.security-groups.public-subnet-1, module.security-groups.public-subnet-2]
  internal        = false
  security_groups = [module.security-groups.api-backoffice-lb-security-group-id]
}

resource "aws_alb_listener" "api-backoffice-listener" {
  load_balancer_arn = aws_alb.api-backoffice-alb.arn
  default_action {
    type             = "forward"
    target_group_arn = aws_alb_target_group.api-backoffice-target-group.arn
  }
  port = 80
}

resource "aws_alb_target_group" "webapp-target-group" {
  name        = "webapp-target-group"
  port        = var.webapp_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.security-groups.vpc-id
}

resource "aws_alb" "webapp-alb" {
  name            = "webapp-alb"
  subnets         = [module.security-groups.public-subnet-1, module.security-groups.public-subnet-2]
  internal        = false
  security_groups = [module.security-groups.webapp-lb-security-group-id]
}

resource "aws_alb_listener" "webapp-listener" {
  load_balancer_arn = aws_alb.webapp-alb.arn
  default_action {
    type             = "forward"
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
  }
  port = 80
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
    image : "${aws_ecr_repository.droits-api-backoffice-repository.repository_url}:${var.api_backoffice_image_tag}",
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
    cpu_architecture        = "ARM64"
  }
}

resource "aws_ecs_service" "backoffice-service" {
  name                              = "backoffice"
  cluster                           = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition                   = aws_ecs_task_definition.backoffice-task-definition.arn
  launch_type                       = "FARGATE"
  desired_count                     = 1
  health_check_grace_period_seconds = 600
  # wait_for_steady_state = true
  depends_on = [
    aws_alb_listener.api-backoffice-listener,
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
    image : "${aws_ecr_repository.droits-webapp-repository.repository_url}:${var.webapp_image_tag}",
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
    cpu_architecture        = "ARM64"
  }
}

resource "aws_ecs_service" "webapp" {
  name                              = "webapp"
  cluster                           = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition                   = aws_ecs_task_definition.webapp-task-definition.arn
  launch_type                       = "FARGATE"
  desired_count                     = 1
  health_check_grace_period_seconds = 600
  # wait_for_steady_state = true
  depends_on = [
    aws_alb_listener.webapp-listener,
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
