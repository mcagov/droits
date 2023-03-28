ecs_cluster_name      = "droits-cluster"
webapp_fargate_cpu    = 256
webapp_fargate_memory = 512
webapp_image_tag      = "latest"

db_name                  = "dev-db"
db_allocated_storage     = 50
db_delete_protection     = false
db_instance_class        = "db.t3.micro"
db_storage_encrypted     = false
api_backoffice_image_tag = "latest"

aws_region       = "eu-west-2"
aws_vpc_id       = "vpc-052a083e9af2d7145"
private_subnet_1 = "subnet-07142266aa23c47b8"
private_subnet_2 = "subnet-06fa9c8603b7dc463"
public_subnet_1  = "subnet-0acd501299e0d6d8a"
public_subnet_2  = "subnet-014306c736144d6f9"

root_domain_name    = "droits.uk"
lb_ssl_policy       = "ELBSecurityPolicy-FS-1-2-2019-08"
ssl_certificate_arn = "arn:aws:acm:eu-west-2:842544458664:certificate/3d9820a3-3e15-46ee-a614-5881a2e9f381"

