terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.27.0"
    }
  }

  backend "s3" {
    bucket = "droits-statefile"
    key = "global/s3/terraform.tfstate"
    encrypt = true
    region = "eu-west-2"
    profile = "default"
  }
}

# Configure the AWS Provider
provider "aws" {
  region = var.aws_region
  access_key = var.aws_access_key_id
  secret_key = var.aws_secret_access_key
}

module "network" {
  source = "./modules/network"
}

module "iam" {
  source = "./modules/iam"
}

module "rds" {
  public_subnet_1 = module.network.public-subnet-1
  public_subnet_2 = module.network.public-subnet-2
  db_security_group_id = module.network.db-security-group-id
  source = "./modules/rds"
}

resource "aws_s3_bucket" "droits-wreck-images"{
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
  name         = var.ecs_cluster_name
}

resource "aws_alb_target_group" "api-backoffice-target-group" {
  name        = "api-backoffice-target-group"
  port        = var.api_backoffice_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.network.vpc-id
}

resource "aws_alb" "api-backoffice-alb" {
  name         = "api-backoffice-alb"
  subnets      = [module.network.public-subnet-1,module.network.public-subnet-2]
  internal     = true
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
  vpc_id      = module.network.vpc-id
}

resource "aws_alb" "webapp-alb" {
  name         = "webapp-alb"
  subnets      = [module.network.public-subnet-1,module.network.public-subnet-2]
  internal     = true
}

resource "aws_alb_listener" "webapp-listener" {
  load_balancer_arn = aws_alb.webapp-alb.arn
  default_action {
    type = "forward"
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
  }
  port              = 81
}

resource "aws_security_group_rule" "load-balancer-to-api-backoffice" {
  from_port         = 80
  protocol          = "tcp"
  security_group_id = module.network.api-backoffice-id
  to_port           = 80
  type              = "ingress"
  cidr_blocks       = ["10.0.0.0/16"]
}

resource "aws_ecs_task_definition" "backoffice-task-definition" {
  family                    = "backoffice"
  execution_role_arn        = module.iam.iam-role-arn
  requires_compatibilities  = ["FARGATE"]
  network_mode              = "awsvpc"
  cpu                       = var.api_backoffice_fargate_cpu
  memory                    = var.api_backoffice_fargate_memory
  container_definitions     = jsonencode([{
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
        "CMD-SHELL", "wget --no-verbose --tries=1 --spider http://localhost:5000/health &>/dev/null || exit 1"
      ],
    }
  }])
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture = "ARM64"
  }
}

resource "aws_ecs_service" "backoffice-service" {
  name            = "api-backoffice-container"
  cluster         = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition = aws_ecs_task_definition.backoffice-task-definition.arn
  launch_type     = "FARGATE"

  network_configuration {
    security_groups = [module.network.api-backoffice-id]
    subnets         = [module.network.public-subnet-1]
  }
  
  load_balancer {
    target_group_arn  = aws_alb_target_group.api-backoffice-target-group.arn
    container_name    = "api-backoffice"
    container_port    = var.api_backoffice_port
  }
}

resource "aws_ecs_task_definition" "webapp-task-definition" {
  family                = "webapp"
  execution_role_arn = module.iam.iam-role-arn
  requires_compatibilities  = ["FARGATE"]
  network_mode              = "awsvpc"
  cpu                       = var.webapp_fargate_cpu
  memory                    = var.webapp_fargate_memory
  container_definitions = jsonencode([{
    name : "webapp",
    image : "${aws_ecr_repository.droits-webapp-repository.repository_url}:${var.webapp_image_tag}",
    portMappings : [
      {
        containerPort : var.webapp_port
        hostPort : var.webapp_port
      }
    ],
    //health check goes here
  }])
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture = "ARM64"
  }
}

resource "aws_ecs_service" "webapp" {
  name = "webapp-container"
  cluster = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition = aws_ecs_task_definition.webapp-task-definition.arn
  launch_type = "FARGATE"
  
  network_configuration {
    security_groups = [module.network.webapp-security-group-id]
    subnets         = [module.network.public-subnet-1]
  }
  
  load_balancer {
    target_group_arn = aws_alb_target_group.webapp-target-group.arn
    container_name = "webapp"
    container_port = var.webapp_port
  }
}