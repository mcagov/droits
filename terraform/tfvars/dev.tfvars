ecs_cluster_name          = "droits-cluster"
webapp_fargate_cpu        = 1024
webapp_fargate_memory     = 2048
backoffice_fargate_cpu    = 2048
backoffice_fargate_memory = 6144

db_name              = "droits-db"
db_allocated_storage = 50
db_delete_protection = true
db_instance_class    = "db.t3.medium"
db_storage_encrypted = true

aws_region = "eu-west-2"

root_domain_name    = "report-wreck-material.service.gov.uk"
lb_ssl_policy       = "ELBSecurityPolicy-FS-1-2-2019-08"
ssl_certificate_arn = "arn:aws:acm:eu-west-2:842544458664:certificate/24e37f5d-35cb-46ff-a087-4085764ca82c"

enable_alerts                                      = true
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
