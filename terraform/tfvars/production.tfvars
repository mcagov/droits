ecs_cluster_name          = "droits-cluster"
webapp_fargate_cpu        = 1024
webapp_fargate_memory     = 2048
backoffice_fargate_cpu    = 2048
backoffice_fargate_memory = 6144

db_name              = "droits-db"
db_allocated_storage = 150
db_delete_protection = true
db_instance_class    = "db.t3.large"
db_storage_encrypted = true

aws_region = "eu-west-2"

root_domain_name    = "report-wreck-material.service.gov.uk"
create_hosted_zone  = true
production_webapp_alb_dns = "webapp-alb-2123332444.eu-west-2.elb.amazonaws.com"
production_backoffice_alb_dns = "backoffice-alb-2102331477.eu-west-2.elb.amazonaws.com"
staging_webapp_alb_dns = "webapp-alb-614194714.eu-west-2.elb.amazonaws.com"
staging_backoffice_alb_dns = "backoffice-alb-1524165112.eu-west-2.elb.amazonaws.com"
dev_webapp_alb_dns = "webapp-alb-1338878440.eu-west-2.elb.amazonaws.com"
dev_backoffice_alb_dns = "backoffice-alb-469192954.eu-west-2.elb.amazonaws.com"

lb_ssl_policy       = "ELBSecurityPolicy-FS-1-2-2019-08"
ssl_certificate_arn = "arn:aws:acm:eu-west-2:257298404318:certificate/b53f260b-bdb1-416f-80df-7c558e1ddb73"

enable_alerts                                      = false
percentage_cpu_utilization_high_threshold          = 90
percentage_memory_utilization_high_threshold       = 90
cpu_utilisation_duration_in_seconds_to_evaluate    = 300
cpu_utilization_high_evaluation_periods            = 1
memory_utilisation_duration_in_seconds_to_evaluate = 300
memory_utilization_high_evaluation_periods         = 1

db_evaluation_periods                              = "1"
db_cpu_credit_balance_too_low_threshold            = "100"
db_maximum_used_transaction_ids_too_high_threshold = "1000000000"
db_memory_freeable_too_low_threshold               = "256000000"
db_memory_swap_usage_too_high_threshold            = "256000000"

lb_average_response_time_threshold = "600"
lb_evaluation_periods              = "1"
