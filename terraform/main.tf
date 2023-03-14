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

resource "aws_alb_target_group" "api-backoffice-target-group" {
  name        = "api-backoffice-target-group"
  port        = var.api_backoffice_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = module.network.vpc-id
}

resource "aws_security_group_rule" "load-balancer-to-api-backoffice" {
  from_port         = 80
  protocol          = "tcp"
  security_group_id = module.network.api-backoffice-id
  to_port           = 80
  type              = "ingress"
  cidr_blocks       = ["10.0.0.0/16"]
}

resource "aws_ecs_task_definition" "backoffice" {
  family                = "backoffice"
  execution_role_arn = module.iam.aws_iam_role.ecs_task_execution.arn
  container_definitions = jsonencode([{
    name : "api-backoffice",
    image : "${data.aws_ecr_repository.droits-api-backoffice-repository.repository_url}:${var.api_backoffice_image_tag}",
    portMappings : [
      {
        containerPort : var.api_backoffice_port
        hostPort : var.api_backoffice_port
      }
    ],
    logConfiguration : {
      "logDriver" : "awslogs",
      "options" : {
        "awslogs-group" : aws_cloudwatch_log_group.log_group.name,
        "awslogs-region" : var.aws_region,
        "awslogs-stream-prefix" : "api-backoffice"
      }
    },
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

resource "aws_ecs_service" "backoffice" {
  name = "api-backoffice-container"
  cluster = aws_ecs_cluster.droits-ecs-cluster.id
  task_definition = aws_ecs_task_definition.backoffice
  launch_type = "FARGATE"
  
  load_balancer {
    target_group_arn = aws_alb_target_group.api-backoffice-target-group.arn
    container_name = "api-backoffice"
    container_port = var.api_backoffice_port
  }
}