public_fqdn                             = ""
az_count                                = 2
ecs_fargate_version                     = "LATEST"
ecs_cluster_name                        = "droits-cluster"
webapp_count                            = 1
webapp_fargate_cpu                      = 256
webapp_fargate_memory                   = 512
webapp_image_tag                        = "latest"
webapp_azure_b2c_tenant_id              = ""
webapp_azure_b2c_client_id              = ""
webapp_azure_b2c_client_secret          = ""
webapp_azure_b2c_tenant_name            = "B2CMCGA"
webapp_azure_b2c_login_flow             = ""
webapp_azure_b2c_signup_flow            = ""
webapp_ecr_repository_name              = "droits-webapp-repository"
azure_ad_tenant_id                      = "513fb495-9a90-425b-a49a-bc6ebe2a429e"
service_count                           = 1
api_backoffice_azure_ad_client_id       = ""
api_backoffice_image_tag                = "latest"
api_backoffice_ecr_repository_name      = "droits_api_backoffice_repository"
apply_immediately                       = true
ssl_certificate_arn                     = "arn:aws:acm:eu-west-2:232705206979:certificate/cca7f7e5-8b98-443d-a6c6-245e7b653200"

aws_region                              = "eu-west-2"
aws_account_number                      = null
aws_access_key_id                       = null
aws_secret_access_key                   = null
aws_session_token                       = null