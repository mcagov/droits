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

provider "aws" {
  region = var.aws_region
  access_key = var.aws_access_key_id
  secret_key = var.aws_secret_access_key
}

module "security-groups" {
  aws_vpc_id = var.aws_vpc_id
  private_subnet_1 = var.private_subnet_1
  private_subnet_2 = var.private_subnet_2
  public_subnet_1 = var.public_subnet_1
  public_subnet_2 = var.public_subnet_2
  source = "./modules/security-groups"
}

module "iam" {
  source = "./modules/iam"
}

module "rds" {
  public_subnet_1 = module.security-groups.public-subnet-1
  public_subnet_2 = module.security-groups.public-subnet-2
  db_security_group_id = module.security-groups.db-security-group-id
  db_delete_protection = var.db_delete_protection
  source = "./modules/rds"
}

module "ecs" {
  ecs_cluster_name = var.ecs_cluster_name
  api_backoffice_port = var.api_backoffice_port
  public_subnet_1 = var.public_subnet_1
  public_subnet_2 = var.public_subnet_2
  api_backoffice_lb_security_group_id = module.security-groups.api-backoffice-lb-security-group-id
  webapp_port = var.webapp_port
  webapp_lb_security_group_id = module.security-groups.webapp-lb-security-group-id
  vpc_id = module.security-groups.vpc-id
  backoffice_security_group = module.security-groups.api-backoffice-id
  webapp_security_group = module.security-groups.webapp-security-group-id
  execution_role_arn = module.iam.iam-role-arn
  webapp_fargate_cpu = var.webapp_fargate_cpu
  webapp_fargate_memory = var.webapp_fargate_memory
  webapp_image_url = "${aws_ecr_repository.droits-webapp-repository.repository_url}:${var.webapp_image_tag}"
  backoffice_fargate_cpu = var.api_backoffice_fargate_cpu
  backoffice_fargate_memory = var.api_backoffice_fargate_memory
  backoffice_image_url = "${aws_ecr_repository.droits-api-backoffice-repository.repository_url}:${var.api_backoffice_image_tag}"
  webapp_target_group_arn = module.alb.webapp-target-group-arn
  api_backoffice_target_group_arn = module.alb.api-backoffice-target-group-arn
  source = "./modules/ecs"
  depends_on = [module.alb]
}

module "alb" {
  source = "./modules/alb"
  api_backoffice_port = var.api_backoffice_port
  public_subnet_1 = var.public_subnet_1
  public_subnet_2 = var.public_subnet_2
  api_backoffice_lb_security_group_id = module.security-groups.api-backoffice-lb-security-group-id
  webapp_port = var.webapp_port
  webapp_lb_security_group_id = module.security-groups.webapp-lb-security-group-id
  vpc_id = module.security-groups.vpc-id
  backoffice_security_group = module.security-groups.api-backoffice-id
  webapp_security_group = module.security-groups.webapp-security-group-id
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