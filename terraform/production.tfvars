ecs_cluster_name      = "droits-cluster"
webapp_fargate_cpu    = 256
webapp_fargate_memory = 512

db_name              = "production-db"
db_allocated_storage = 50
db_delete_protection = false
db_instance_class    = "db.t3.micro"
db_storage_encrypted = false

aws_region       = "eu-west-2"
aws_vpc_id       = ""
private_subnet_1 = ""
private_subnet_2 = ""
public_subnet_1  = ""
public_subnet_2  = ""

root_domain_name    = "droits.uk"
lb_ssl_policy       = ""
ssl_certificate_arn = ""

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

lb_response_time_threshold = "600"
lb_evaluation_periods      = "1"