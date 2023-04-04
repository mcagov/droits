ecs_cluster_name              = "droits-cluster"
webapp_fargate_cpu            = 2048
webapp_fargate_memory         = 4096
api_backoffice_fargate_cpu    = 256
api_backoffice_fargate_memory = 512

db_name              = "dev-db"
db_allocated_storage = 50
db_delete_protection = false
db_instance_class    = "db.t3.micro"
db_storage_encrypted = false

aws_region       = "eu-west-2"
aws_vpc_id       = "vpc-052a083e9af2d7145"
private_subnet_1 = "subnet-07142266aa23c47b8"
private_subnet_2 = "subnet-06fa9c8603b7dc463"
public_subnet_1  = "subnet-0acd501299e0d6d8a"
public_subnet_2  = "subnet-014306c736144d6f9"

root_domain_name    = "droits.uk"
lb_ssl_policy       = "ELBSecurityPolicy-FS-1-2-2019-08"
ssl_certificate_arn = "arn:aws:acm:eu-west-2:842544458664:certificate/3d9820a3-3e15-46ee-a614-5881a2e9f381"
regional_account_id = "652711504416"

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

lb_response_time_threshold = "600"
lb_evaluation_periods      = "1"
