terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.27.0"
    }
  }

  backend "s3" {
    key     = "global/s3/terraform.tfstate"
    encrypt = true
    region  = "eu-west-2"
  }
}

provider "aws" {
  region = var.aws_region
}

module "vpc" {
  source = "./modules/vpc"
}

module "security-groups" {
  source           = "./modules/security-groups"
  vpc_id           = module.vpc.vpc_id
  redis_port       = var.redis_port
  application_name = var.webapp_ecr_repository_name

  depends_on = [module.vpc]
}


module "iam" {
  source               = "./modules/iam"
  s3_images_bucket_arn = module.s3-images.bucket_arn

  depends_on = [module.s3-images]
}

module "acm" {
  source      = "./modules/acm"
  ssl_domains = var.ssl_domains
}

module "rds" {
  source               = "./modules/rds"
  vpc_id               = module.vpc.vpc_id
  public_subnets       = module.vpc.public_subnets
  private_subnets      = module.vpc.private_subnets
  db_delete_protection = var.db_delete_protection
  db_password          = var.db_password
  db_username          = var.db_username
  db_instance_class    = var.db_instance_class
  db_security_groups   = [module.security-groups.db-security-group-id]

  depends_on = [module.vpc, module.security-groups]
}

module "backoffice-alb" {
  source = "./modules/alb"

  vpc_id          = module.vpc.vpc_id
  public_subnets  = module.vpc.public_subnets
  private_subnets = module.vpc.private_subnets

  lb_ssl_policy       = var.lb_ssl_policy
  ssl_certificate_arn = module.acm.ssl_certificate_arn

  port             = var.backoffice_port
  protocol         = "HTTP"
  security_groups  = [module.security-groups.backoffice-lb-security-group-id]
  lb_log_bucket    = module.backoffice-logs-s3.alb-log-bucket
  application_name = "backoffice"

  depends_on = [module.vpc, module.security-groups, module.backoffice-logs-s3]
}

module "webapp-alb" {
  source = "./modules/alb"

  vpc_id          = module.vpc.vpc_id
  public_subnets  = module.vpc.public_subnets
  private_subnets = module.vpc.private_subnets

  lb_ssl_policy       = var.lb_ssl_policy
  ssl_certificate_arn = module.acm.ssl_certificate_arn


  port             = var.webapp_port
  protocol         = "HTTP"
  security_groups  = [module.security-groups.webapp-lb-security-group-id]
  lb_log_bucket    = module.webapp-logs-s3.alb-log-bucket
  application_name = "webapp"

  depends_on = [module.vpc, module.security-groups, module.webapp-logs-s3]
}

module "droits-ecs-cluster" {
  source = "./modules/ecs-cluster"
  name   = var.ecs_cluster_name
}

module "backoffice-ecs" {
  source             = "./modules/ecs"
  service_name       = "backoffice"
  health_check_url   = "http://127.0.0.1:5000/healthz"
  aws_region         = var.aws_region
  vpc_id             = module.vpc.vpc_id
  public_subnets     = module.vpc.public_subnets
  private_subnets    = module.vpc.private_subnets
  execution_role_arn = module.iam.iam_execution_role_arn
  task_role_arn      = module.iam.iam_task_role_arn
  security_groups    = [module.security-groups.backoffice-id]
  tg_arn             = module.backoffice-alb.target-group-arn
  droits_ecs_cluster = module.droits-ecs-cluster.ecs_cluster_id
  port               = var.backoffice_port
  fargate_cpu        = var.backoffice_fargate_cpu
  fargate_memory     = var.backoffice_fargate_memory
  image_url          = "${var.ecr_repository_url}/${var.backoffice_ecr_repository_name}:${var.image_tag}"
  environment_file   = var.backoffice_environment_file

  depends_on = [module.vpc, module.iam, module.backoffice-alb, module.droits-ecs-cluster]
}

module "webapp-ecs" {
  source             = "./modules/ecs"
  service_name       = "webapp"
  health_check_url   = "http://127.0.0.1:3000/health"
  aws_region         = var.aws_region
  vpc_id             = module.vpc.vpc_id
  public_subnets     = module.vpc.public_subnets
  private_subnets    = module.vpc.private_subnets
  execution_role_arn = module.iam.iam_execution_role_arn
  task_role_arn      = module.iam.iam_task_role_arn
  security_groups    = [module.security-groups.webapp-security-group-id]
  tg_arn             = module.webapp-alb.target-group-arn
  droits_ecs_cluster = module.droits-ecs-cluster.ecs_cluster_id
  port               = var.webapp_port
  fargate_cpu        = var.webapp_fargate_cpu
  fargate_memory     = var.webapp_fargate_memory
  image_url          = "${var.ecr_repository_url}/${var.webapp_ecr_repository_name}:${var.image_tag}"
  environment_file   = var.webapp_environment_file

  depends_on = [module.vpc, module.iam, module.webapp-alb, module.droits-ecs-cluster]
}

module "backoffice-sns" {
  source                          = "./modules/sns"
  resource_name                   = "backoffice"
  alert_email_address             = var.alert_email_address
  aws_account_number              = var.aws_account_number
  alert_pagerduty_integration_url = var.alert_pagerduty_integration_url
}
module "backoffice-lb-sns" {
  source                          = "./modules/sns"
  resource_name                   = "backoffice-lb"
  alert_email_address             = var.alert_email_address
  aws_account_number              = var.aws_account_number
  alert_pagerduty_integration_url = var.alert_pagerduty_integration_url
}
module "webapp-sns" {
  source                          = "./modules/sns"
  resource_name                   = "webapp"
  alert_email_address             = var.alert_email_address
  aws_account_number              = var.aws_account_number
  alert_pagerduty_integration_url = var.alert_pagerduty_integration_url
}
module "webapp-lb-sns" {
  source                          = "./modules/sns"
  resource_name                   = "webapp-lb"
  alert_email_address             = var.alert_email_address
  aws_account_number              = var.aws_account_number
  alert_pagerduty_integration_url = var.alert_pagerduty_integration_url
}
module "db-sns" {
  source                          = "./modules/sns"
  resource_name                   = "db"
  alert_email_address             = var.alert_email_address
  aws_account_number              = var.aws_account_number
  alert_pagerduty_integration_url = var.alert_pagerduty_integration_url
}

module "elasticache" {
  source           = "./modules/elasticache"
  vpc_id           = module.vpc.vpc_id
  redis_port       = var.redis_port
  public_subnets   = module.vpc.public_subnets
  application_name = var.webapp_ecr_repository_name
  security_groups  = [module.security-groups.elasticache-security-group-id]

  depends_on = [module.vpc, module.security-groups]
}

module "cloudwatch" {
  source     = "./modules/cloudwatch"
  aws_region = var.aws_region

  backoffice_load_balancer       = module.backoffice-alb.alb-name
  backoffice_alb_id              = module.backoffice-alb.alb-id
  backoffice_alb_arn_suffix      = module.backoffice-alb.alb-arn-suffix
  backoffice_alb_target_group_id = module.backoffice-alb.target-group-id
  webapp_load_balancer           = module.webapp-alb.alb-name
  webapp_alb_id                  = module.webapp-alb.alb-id
  webapp_alb_arn_suffix          = module.webapp-alb.alb-arn-suffix
  webapp_alb_target_group_id     = module.webapp-alb.target-group-id

  ecs_cluster_name            = var.ecs_cluster_name
  ecs_backoffice_service_name = "backoffice"
  ecs_webapp_service_name     = "webapp"

  rds_instance_identifier = module.rds.instance-identifier
  db_instance_id          = module.rds.instance-identifier

  ecs_backoffice_alerts_topic_arn = module.backoffice-sns.alerts-topic-arn
  ecs_webapp_alerts_topic_arn     = module.webapp-sns.alerts-topic-arn
  rds_db_alerts_topic_arn         = module.db-sns.alerts-topic-arn
  backoffice_lb_alerts_topic_arn  = module.backoffice-lb-sns.alerts-topic-arn
  webapp_lb_alerts_topic_arn      = module.webapp-lb-sns.alerts-topic-arn

  db_instance_class                                  = var.db_instance_class
  db_low_disk_burst_balance_threshold                = var.db_low_disk_burst_balance_threshold
  enable_alerts                                      = var.enable_alerts
  ecs_webapp_service_minimum_task_count              = var.webapp_service_minimum_task_count
  db_cpu_utilization_high_threshold_percentage       = var.percentage_cpu_utilization_high_threshold
  db_cpu_credit_balance_too_low_threshold            = var.db_cpu_credit_balance_too_low_threshold
  lb_average_response_time_threshold                 = var.lb_average_response_time_threshold
  db_memory_swap_usage_too_high_threshold            = var.db_memory_swap_usage_too_high_threshold
  db_maximum_used_transaction_ids_too_high_threshold = var.db_maximum_used_transaction_ids_too_high_threshold
  lb_evaluation_periods                              = var.lb_evaluation_periods
  db_memory_freeable_too_low_threshold               = var.db_memory_freeable_too_low_threshold
  db_evaluation_periods                              = var.db_evaluation_periods

}

module "s3-images" {
  source = "./modules/s3-images"
}

module "backoffice-logs-s3" {
  source              = "./modules/s3-logs"
  regional_account_id = var.regional_account_id
  application_name    = "backoffice"
}

module "webapp-logs-s3" {
  source              = "./modules/s3-logs"
  regional_account_id = var.regional_account_id
  application_name    = "webapp"
}

module "route53" {
  source                    = "./modules/route53"
  root_domain_name          = var.root_domain_name
  a_records                 = var.a_records
  delegated_hosted_zones    = var.delegated_hosted_zones
  webapp_alb_dns            = module.webapp-alb.alb-dns
  backoffice_alb_dns        = module.backoffice-alb.alb-dns
  domain_validation_options = module.acm.domain_validation_options
  ssl_certificate_arn       = module.acm.ssl_certificate_arn
}
