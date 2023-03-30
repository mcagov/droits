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
  source           = "./modules/security-groups"
  aws_vpc_id       = var.aws_vpc_id
  private_subnet_1 = var.private_subnet_1
  private_subnet_2 = var.private_subnet_2
  public_subnet_1  = var.public_subnet_1
  public_subnet_2  = var.public_subnet_2
}

module "iam" {
  source = "./modules/iam"
}

module "rds" {
  source               = "./modules/rds"
  public_subnet_1      = module.security-groups.public-subnet-1
  public_subnet_2      = module.security-groups.public-subnet-2
  db_security_group_id = module.security-groups.db-security-group-id
  db_delete_protection = var.db_delete_protection
  db_password          = var.db_password
  db_username          = var.db_username
  db_instance_class    = var.db_instance_class
}

module "cloudwatch" {
  source                                   = "./modules/cloudwatch"
  ecs_cluster_name                         = aws_ecs_cluster.droits-ecs-cluster.name
  ecs_backoffice_service_name              = aws_ecs_service.backoffice-service.name
  ecs_webapp_service_name                  = aws_ecs_service.webapp.name
  rds_instance_identifier                  = module.rds.instance_identifier
  aws_region                               = var.aws_region
  backoffice_load_balancer                 = aws_alb.api-backoffice-alb.name
  backoffice_alb_id                        = aws_alb.api-backoffice-alb.id
  backoffice_alb_target_group_id           = aws_alb_target_group.api-backoffice-target-group.id
  webapp_load_balancer                     = aws_alb.webapp-alb.name
  webapp_alb_id                            = aws_alb.webapp-alb.id
  webapp_alb_target_group_id               = aws_alb_target_group.webapp-target-group.id
  db_instance_id                           = module.rds.instance_identifier
  db_instance_class                        = var.db_instance_class
  db_low_disk_burst_balance_threshold      = var.db_low_disk_burst_balance_threshold
  enable_alerts                            = var.enable_alerts
  ecs_backofice_service_minimum_task_count = var.api_backofice_service_minimum_task_count
  ecs_backoffice_alerts_topic_arn          = module.sns.backoffice_alerts_topic_arn
  ecs_webapp_service_minimum_task_count    = var.webapp_service_minimum_task_count
}

module "sns" {
  source              = "./modules/sns"
  alert_email_address = var.alert_email_address
  aws_account_number  = var.aws_account_number
}

module "s3" {
  source              = "./modules/s3"
  regional_account_id = var.regional_account_id
}

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