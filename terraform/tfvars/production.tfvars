ecs_cluster_name          = "droits-cluster"
webapp_fargate_cpu        = 512
webapp_fargate_memory     = 1024
backoffice_fargate_cpu    = 512
backoffice_fargate_memory = 1024


db_name              = "droits-db"
db_allocated_storage = 50
db_delete_protection = false
db_instance_class    = "db.t3.micro"
db_storage_encrypted = false

aws_region = "eu-west-2"

root_domain_name    = "droits.uk"
lb_ssl_policy       = "ELBSecurityPolicy-FS-1-2-2019-08"
ssl_certificate_arn = "arn:aws:acm:eu-west-2:257298404318:certificate/2b6b805d-32c7-4597-b0c9-93bad76f4f0c"

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
