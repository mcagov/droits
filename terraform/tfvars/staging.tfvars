ecs_cluster_name      = "droits-cluster"
webapp_fargate_cpu    = 256
webapp_fargate_memory = 512

db_name              = "staging-db"
db_allocated_storage = 50
db_delete_protection = false
db_instance_class    = "db.t3.micro"
db_storage_encrypted = false

aws_region       = "eu-west-2"
aws_vpc_id       = "vpc-098229da848dcdbb1"
private_subnet_1 = "subnet-0c3e36c34dd274194"
private_subnet_2 = "subnet-073565f84c47bbd6b"
public_subnet_1  = "subnet-08d45160f619b6052"
public_subnet_2  = "subnet-033c708267c904683"

root_domain_name    = "droits.uk"
lb_ssl_policy       = "ELBSecurityPolicy-FS-1-2-2019-08"
ssl_certificate_arn = "arn:aws:acm:eu-west-2:703203758589:certificate/c5ffdfbb-8e53-4501-abd9-1005426cd3ad"

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
