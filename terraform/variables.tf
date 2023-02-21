variable "public_fqdn" {
  type        = string
  description = "The fully qualified domain name of the publicly accessible parts of the application"
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
  default     = 80
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