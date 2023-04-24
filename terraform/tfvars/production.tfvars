ecs_cluster_name      = "droits-cluster"
webapp_fargate_cpu    = 256
webapp_fargate_memory = 512

db_name              = "production-db"
db_allocated_storage = 50
db_delete_protection = false
db_instance_class    = "db.t3.micro"
db_storage_encrypted = false

aws_region       = "eu-west-2"
aws_vpc_id       = "vpc-0bd4b6e2795acc5c3"
private_subnet_1 = "subnet-097aeba4ed6243952"
private_subnet_2 = "subnet-0215265a0e11ca9d0"
public_subnet_1  = "subnet-08a50f654480c3a95"
public_subnet_2  = "subnet-07d91f41b17a3aac5"

root_domain_name    = "droits.uk"
lb_ssl_policy       = "ELBSecurityPolicy-FS-1-2-2019-08"
ssl_certificate_arn = "arn:aws:acm:eu-west-2:257298404318:certificate/a2be611b-9220-41e1-8761-5b5f5a7b3d5b"

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
