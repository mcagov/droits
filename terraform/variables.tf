
variable "aws_region" {
  type        = string
  description = "The AWS region resources are created in"
  default     = "eu-west-2"
}

variable "ecs_cluster_name" {
  type        = string
  description = "The name of the ECS Fargate cluster"
}
variable "webapp_ecr_repository_name" {
  type        = string
  description = "The name of the Elastic Container Repository for our webapp container images"
}
variable "webapp_port" {
  type        = number
  description = "Port exposed by the docker image to redirect traffic to for the DROITS Webapp"
  default     = 3000
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
  default     = 2048
}
variable "webapp_service_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the webapp ECS service"
  default     = 1
}

variable "enable_alerts" {
  type        = bool
  description = "When enabled CloudWatch alarm events are sent to the Alerts SNS Topic"
  default     = false
}

variable "root_domain_name" {
  type        = string
  description = "The root domain name for DROITS"
}

variable "lb_ssl_policy" {
  type        = string
  description = "Security policy for the SSL certificate"
}
variable "ssl_certificate_arn" {
  type        = string
  description = "ARN of ssl certificate"
}

variable "api_backoffice_image" {
  type        = string
  description = "The name of the image for the Backoffice application"
  default     = "DROITS-api-backoffice"
}
variable "image_tag" {
  type        = string
  description = "The image tag of either application to be deployed"
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
  default     = "/healthz"
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
  type        = string
  description = "The name of the Elastic Container Repository for our api-backoffice container images"
}
variable "api_backofice_service_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the backoffice ECS service"
  default     = 1
}

variable "ecr_repository_url" {
  type        = string
  description = "The url of the Elastic Container Repository for our container images"
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

variable "db_delete_protection" {
  type        = bool
  description = "Database protection setting"
}

variable "db_username" {
  type        = string
  description = "The username for the master database user"
  default     = "droits"
  sensitive   = true
}
variable "db_password" {
  type        = string
  description = "The password used for the master database user"
  sensitive   = true
  default     = ""
}

variable "db_instance_class" {
  type        = string
  description = "The database instance class"
}

variable "db_allocated_storage" {
  type        = number
  description = "Allocated storage, in GB, for the DB instance"
}

variable "db_storage_encrypted" {
  type        = bool
  description = "Specifies whether the database instances data is encrypted"
}
variable "db_name" {
  type        = string
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
variable "regional_account_id" {
  type        = string
  description = "The id of the region we are currently deploying to"
}
variable "db_low_disk_burst_balance_threshold" {
  type        = number
  default     = 100
  description = "Alarm threshold for low RDS disk burst balance"
}
variable "alert_email_address" {
  sensitive   = true
  type        = string
  description = "Email Address subscribed to alerts"
}