public_fqdn                    = ""
az_count                       = 2
ecs_fargate_version            = "LATEST"
ecs_cluster_name               = "droits-cluster"
webapp_count                   = 1
webapp_fargate_cpu             = 256
webapp_fargate_memory          = 512
webapp_image_tag               = "latest"
webapp_azure_b2c_tenant_id     = ""
webapp_azure_b2c_client_id     = ""
webapp_azure_b2c_client_secret = ""
webapp_azure_b2c_tenant_name   = "B2CMCGA"
webapp_azure_b2c_login_flow    = ""
webapp_azure_b2c_signup_flow   = ""
azure_ad_tenant_id             = "513fb495-9a90-425b-a49a-bc6ebe2a429e"
service_count                  = 1

api_backoffice_azure_ad_client_id = ""
db_name                           = "dev-db"
db_allocated_storage              = 50
db_max_storage                    = 50
db_delete_protection              = false
db_instance_class                 = "db.t3.micro"
db_storage_encrypted              = false
nat_gateway_count                 = 1
backup_window                     = "23:00-23:55"
backup_retention_period           = 5
performance_insights_enabled      = true
apply_immediately                 = true
api_service_minimum_task_count    = 1
webapp_minimum_task_count         = 1
gov_notify_feedback_email_address = "beacons_test_feedback@mailsac.com"
low_disk_burst_balance_threshold  = 75
ssl_certificate_arn               = "arn:aws:acm:eu-west-2:232705206979:certificate/cca7f7e5-8b98-443d-a6c6-245e7b653200"
api_backoffice_image_tag          = "latest"

aws_region       = "eu-west-2"
aws_vpc_id       = "vpc-052a083e9af2d7145"
private_subnet_1 = "subnet-07142266aa23c47b8"
private_subnet_2 = "subnet-06fa9c8603b7dc463"
public_subnet_1  = "subnet-0acd501299e0d6d8a"
public_subnet_2  = "subnet-014306c736144d6f9"
