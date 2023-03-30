ecs_cluster_name      = "droits-cluster"
webapp_fargate_cpu    = 256
webapp_fargate_memory = 512

db_name              = "staging-db"
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

enable_alerts       = false