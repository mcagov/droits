variable "public_fqdn" {
  type        = string
  description = "The fully qualified domain name of the publicly accessible parts of the application"
}
variable "gov_notify_feedback_email_address" {
  type        = string
  description = "Email address for Gov Notify feedback"
}
variable "aws_region" {
  type        = string
  description = "The AWS region resources are created in"
  default = "eu-west-2"
}
variable "az_count" {
  type        = number
  description = "Number of AZs to cover in a given region"
}
# See docs: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/platform_versions.html
variable "ecs_fargate_version" {
  type        = string
  description = "The version of fargate to run the ECS tasks on"
}
variable "ecs_cluster_name" {
  type        = string
  description = "The name of the ECS Fargate cluster"
}
variable "webapp_image" {
  type        = string
  description = "Docker image to run in the ECS cluster for the DROITS Webapp"
  default     = "DROITS-webapp"
}
variable "webapp_image_tag" {
  type        = string
  description = "Hash of the relevant commit to the mca-droits repo"
}
variable "webapp_port" {
  type        = number
  description = "Port exposed by the docker image to redirect traffic to for the DROITS Webapp"
  default     = 3000
}
variable "webapp_count" {
  type        = number
  description = "Number of docker containers to run for the DROITS Webapp"
}
variable "webapp_health_check_path" {
  type        = string
  description = "Health check path used by the Application Load Balancer for the DROITS Webapp"
  default     = "/api/health"
}
// See docs for ecs task definition: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/task_definition_parameters.html
variable "webapp_fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the DROITS Webapp"
  default     = 1024
}
variable "webapp_fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the DROITS Webapp"
  default = 2048
}
variable "azure_ad_tenant_id" {
  sensitive   = true
  type        = string
  description = "The UUID for the Azure AD tenant, provided in Azure AD"
}
variable "webapp_azure_b2c_client_id" {
  type        = string
  description = "The Azure B2C Client ID for the B2C App Registration"
}
variable "webapp_azure_b2c_client_secret" {
  type        = string
  sensitive   = true
  description = "The client secret for the B2C App Registration"
}
variable "webapp_azure_b2c_tenant_name" {
  type        = string
  description = "The name of the Azure B2C tenant"
}
variable "webapp_azure_b2c_tenant_id" {
  type        = string
  description = "The UUID for the Azure B2C tenant"
}
variable "webapp_azure_b2c_login_flow" {
  type        = string
  description = "The Sign In User Flow defined in Azure B2C"
}
variable "webapp_azure_b2c_signup_flow" {
  type        = string
  description = "The Sign Up User Flow defined in Azure B2C"
}
variable "webapp_ecr_repository_name" {
  type = string
  description = "The name of the Elastic Container Repository for our webapp container images"
}
variable "service_count" {
  type        = number
  description = "Number of docker containers to run for the DROITS Service"
}
// See docs for ecs task definition: https://docs.aws.amazon.com/AmazonECS/latest/developerguide/task_definition_parameters.html
variable "apply_immediately" {
  type        = bool
  description = "Apply changes to infrastrucure immediatly"
  default     = true
}
variable "enable_alerts" {
  type        = bool
  description = "When enabled CloudWatch alarm events are sent to the Alerts SNS Topic"
  default     = false
}
variable "low_disk_burst_balance_threshold" {
  type        = number
  description = "Alarm threshold for low RDS disk burst balance"
  default     = 100
}
variable "api_service_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the API Service"
  default     = 1
}
variable "webapp_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the Webapp"
  default     = 1
}
variable "ssl_certificate_arn" {
  type        = string
  description = "ARN of ssl certificate generated in the AWS dashboard"
}
variable "api_backoffice_azure_ad_client_id" {
  type        = string
  description = "The Client ID of the app registration in Azure AD for the Backoffice MVC app"
}
variable "api_backoffice_image" {
  type        = string
  description = "The name of the image for the Backoffice application"
  default     = "DROITS-api-backoffice"
}
variable "api_backoffice_image_tag" {
  type        = string
  description = "The image tag of the Backoffice application to be deployed"
}
variable "api_backoffice_count" {
  type        = number
  description = "Number of docker containers to run for the Backoffice application"
  default     = 1
}
variable "api_backoffice_port" {
  type        = number
  description = "Port exposed by the docker image to redirect traffic to for the DROITS Service"
  default     = 5000
}
variable "api_backoffice_health_check_path" {
  type        = string
  description = "Health check path used by the Application Load Balancer for the Backoffice app"
  default     = "/health"
}
variable "api_backoffice_fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the Backoffice app"
  default     = 256
}
variable "api_backoffice_fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the Backoffice app"
  default     = 512
}
variable "api_backoffice_ecr_repository_name" {
  type = string
  description = "The name of the Elastic Container Repository for our api-backoffice container images"
}
variable "aws_account_number" {
  sensitive   = true
  type        = string
  description = "The MCA's AWS account number"
  default     = ""
}
variable "aws_access_key_id" {
  sensitive   = true
  type        = string
  description = "The id of the access key for AWS authentication"
  default     = ""  
}
variable "aws_secret_access_key" {
  sensitive   = true
  type        = string
  description = "The secret value of the access key for AWS authentication"
  default     = ""  
}
variable "aws_session_token" {
  sensitive   = true
  type        = string
  description = "The session token used for AWS authentication"
  default     = ""  
}
variable "vpc_resource_id" {
  type        = string
  description = "The id of the Virtual Private Cloud resource in this environment"
  default     = ""
}
variable "db_username" {
  sensitive = true
  type        = string
  description = "The username to authenticate with the DB"
  default     = ""
}
variable "db_password" {
  sensitive = true
  type        = string
  description = "The password to authenticate with the DB"
  default     = ""
}
variable "db_delete_protection" {
  type        = bool
  description = "Database protection setting"
}
variable "backup_window" {
  type        = string
  description = "Timeframe e.g 23:00 - 23:01"
}
variable "db_instance_class" {
  type        = string
  description = "The database instance class"
}
variable "nat_gateway_count" {
  type        = number
  description = "Number of NAT gateways"
  default     = 2
}
variable "performance_insights_enabled" {
  type        = bool
  description = "Enable performance insights"
  default     = false
}
variable "db_max_storage" {
  type        = number
  description = "The upper limit, in GB, to which the storage of the DB can be autoscaled"
}
variable "db_allocated_storage" {
  type        = number
  description = "Allocated storage, in GB, for the DB instance"
}
variable "backup_retention_period" {
  type        = number
  description = "Days to retain backups"
  default     = 0
}
variable "db_storage_encrypted" {
  type        = bool
  description = "Specifies whether the database instances data is encrypted"
}
variable "db_name" {
  type = string
  description = "The name of the DB"
}

variable "aws_vpc_id" {
  type        = string
  description = "ID of main droits vpc"
}
variable "private_subnet_1" {
  type        = string
  description = "ID of first private subnet"
}
variable "private_subnet_2" {
  type        = string
  description = "ID of second private subnet"
}
variable "public_subnet_1" {
  type        = string
  description = "ID of first public subnet"
}
variable "public_subnet_2" {
  type        = string
  description = "ID of second public subnet"
}