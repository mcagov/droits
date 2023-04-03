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

module "backoffice-sns" {
  source              = "./modules/sns"
  resource_name       = "backoffice"
  alert_email_address = var.alert_email_address
  aws_account_number  = var.aws_account_number
}
module "backoffice-lb-sns" {
  source              = "./modules/sns"
  resource_name       = "backoffice-lb"
  alert_email_address = var.alert_email_address
  aws_account_number  = var.aws_account_number
}
module "webapp-sns" {
  source              = "./modules/sns"
  resource_name       = "webapp"
  alert_email_address = var.alert_email_address
  aws_account_number  = var.aws_account_number
}
module "webapp-lb-sns" {
  source              = "./modules/sns"
  resource_name       = "webapp-lb"
  alert_email_address = var.alert_email_address
  aws_account_number  = var.aws_account_number
}
module "db-sns" {
  source              = "./modules/sns"
  resource_name       = "db"
  alert_email_address = var.alert_email_address
  aws_account_number  = var.aws_account_number
}

module "cloudwatch" {
  source                                             = "./modules/cloudwatch"
  ecs_cluster_name                                   = aws_ecs_cluster.droits-ecs-cluster.name
  ecs_backoffice_service_name                        = aws_ecs_service.backoffice-service.name
  ecs_webapp_service_name                            = aws_ecs_service.webapp.name
  rds_instance_identifier                            = module.rds.instance-identifier
  aws_region                                         = var.aws_region
  backoffice_load_balancer                           = aws_alb.api-backoffice-alb.name
  backoffice_alb_id                                  = aws_alb.api-backoffice-alb.id
  backoffice_alb_arn_suffix                          = aws_alb.api-backoffice-alb.arn_suffix
  backoffice_alb_target_group_id                     = aws_alb_target_group.api-backoffice-target-group.id
  webapp_load_balancer                               = aws_alb.webapp-alb.name
  webapp_alb_id                                      = aws_alb.webapp-alb.id
  webapp_alb_arn_suffix                              = aws_alb.webapp-alb.arn_suffix
  webapp_alb_target_group_id                         = aws_alb_target_group.webapp-target-group.id
  db_instance_id                                     = module.rds.instance-identifier
  db_instance_class                                  = var.db_instance_class
  db_low_disk_burst_balance_threshold                = var.db_low_disk_burst_balance_threshold
  enable_alerts                                      = var.enable_alerts
  ecs_backofice_service_minimum_task_count           = var.api_backofice_service_minimum_task_count
  ecs_webapp_service_minimum_task_count              = var.webapp_service_minimum_task_count
  ecs_backoffice_alerts_topic_arn                    = module.backoffice-sns.alerts-topic-arn
  ecs_webapp_alerts_topic_arn                        = module.webapp-sns.alerts-topic-arn
  rds_db_alerts_topic_arn                            = module.db-sns.alerts-topic-arn
  backoffice_lb_alerts_topic_arn                     = module.backoffice-lb-sns.alerts-topic-arn
  webapp_lb_alerts_topic_arn                         = module.webapp-lb-sns.alerts-topic-arn
  percentage_cpu_utilization_high_threshold          = var.percentage_cpu_utilization_high_threshold
  percentage_memory_utilization_high_threshold       = var.percentage_memory_utilization_high_threshold
  cpu_utilisation_duration_in_seconds_to_evaluate    = var.cpu_utilisation_duration_in_seconds_to_evaluate
  db_cpu_credit_balance_too_low_threshold            = var.db_cpu_credit_balance_too_low_threshold
  memory_utilization_high_evaluation_periods         = var.memory_utilization_high_evaluation_periods
  lb_response_time_threshold                         = var.lb_response_time_threshold
  cpu_utilization_high_evaluation_periods            = var.cpu_utilization_high_evaluation_periods
  db_memory_swap_usage_too_high_threshold            = var.db_memory_swap_usage_too_high_threshold
  db_maximum_used_transaction_ids_too_high_threshold = var.db_maximum_used_transaction_ids_too_high_threshold
  lb_evaluation_periods                              = var.lb_evaluation_periods
  memory_utilisation_duration_in_seconds_to_evaluate = var.memory_utilisation_duration_in_seconds_to_evaluate
  db_memory_freeable_too_low_threshold               = var.db_memory_freeable_too_low_threshold
  db_evaluation_periods                              = var.db_evaluation_periods
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