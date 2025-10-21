
variable "aws_region" {
  type        = string
  description = "The AWS region resources are created in"
  default     = "eu-west-2"
}

variable "ecs_cluster_name" {
  type        = string
  description = "The name of the ECS Fargate cluster"
}

variable "image_tag" {
  type        = string
  description = "The name of the Elastic Container Repository for our webapp container images"
}
variable "webapp_ecr_repository_name" {
  sensitive   = true
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
  default     = "/health"
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

variable "backoffice_environment_file" {
  sensitive   = true
  type        = string
  description = "The environment file for the backoffice ECS container"
}

variable "webapp_environment_file" {
  sensitive   = true
  type        = string
  description = "The environment file for the backoffice ECS container"
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

variable "a_records" {
  type = list(object({
    name        = string
    application = string
  }))
  description = "List of A records for Route 53 DNS"
}

variable "delegated_hosted_zones" {
  type        = map(list(string))
  default     = {}
  description = "Map of delegated hosted zones for dev and staging. Eg {domain_name: nameservers}"
}

variable "ssl_domains" {
  type        = list(string)
  description = "List of domains for SSL certificate"
}

variable "lb_ssl_policy" {
  type        = string
  description = "Security policy for the SSL certificate"
}

variable "backoffice_image" {
  type        = string
  description = "The name of the image for the Backoffice application"
  default     = "DROITS-backoffice"
}
variable "backoffice_count" {
  type        = number
  description = "Number of docker containers to run for the Backoffice application"
  default     = 1
}
variable "backoffice_port" {
  type        = number
  description = "Port exposed by the docker image to redirect traffic to for the DROITS Service"
  default     = 5000
}
variable "backoffice_health_check_path" {
  type        = string
  description = "Health check path used by the Application Load Balancer for the Backoffice app"
  default     = "/healthz"
}
variable "backoffice_fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the Backoffice app"
  default     = 256
}
variable "backoffice_fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the Backoffice app"
  default     = 512
}
variable "backoffice_ecr_repository_name" {
  sensitive   = true
  type        = string
  description = "The name of the Elastic Container Repository for our backoffice container images"
}
variable "api_backofice_service_minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the backoffice ECS service"
  default     = 1
}

variable "redis_port" {
  type        = number
  description = "Port exposed by Docker for the Redis session storage"
  default     = 6379
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

variable "regional_account_id" {
  type        = string
  description = "The id of the region we are currently deploying to"
  default     = "652711504416"
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
variable "alert_pagerduty_integration_url" {
  type        = string
  description = " The Integration URL to which CloudWatch alerts should be sent for PagerDuty"
}
variable "percentage_cpu_utilization_high_threshold" {
  type        = number
  description = "The % CPU utilisation limit which, when passed, will trigger an alarm. This will be higher for dev and lower for production."
}
variable "percentage_memory_utilization_high_threshold" {
  type        = number
  description = "The % memory utilisation limit which, when passed, will trigger an alarm. This will be higher for dev and lower for production."
}
variable "cpu_utilization_high_evaluation_periods" {
  type        = number
  description = "Number of periods to evaluate for the alarm"
}
variable "memory_utilization_high_evaluation_periods" {
  type        = number
  description = "Number of periods to evaluate for the alarm"
}
variable "memory_utilisation_duration_in_seconds_to_evaluate" {
  type        = number
  description = "Duration in seconds to evaluate for the alarm"
}
variable "cpu_utilisation_duration_in_seconds_to_evaluate" {
  type        = number
  description = "Duration in seconds to evaluate for the alarm"
}
variable "db_evaluation_periods" {
  type        = string
  description = "The number of periods to evaluate for the alarm"
}
variable "db_cpu_credit_balance_too_low_threshold" {
  type        = string
  description = "Threshold for the DB credit balance too low alarm"
}
variable "db_memory_freeable_too_low_threshold" {
  type        = string
  description = "Threshold for the DB freeable memory too low alarm"
}
variable "db_memory_swap_usage_too_high_threshold" {
  type        = string
  description = "Threshold for the DB memory swap usage too high alarm"
}
variable "db_maximum_used_transaction_ids_too_high_threshold" {
  type        = string
  description = "Threshold for the maximum used transaction IDs DB alarm"
}
variable "lb_average_response_time_threshold" {
  type        = string
  description = "The average number of milliseconds that requests should complete within"
}
variable "lb_evaluation_periods" {
  type        = string
  description = "The number of periods to evaluate for the alarm"
}
