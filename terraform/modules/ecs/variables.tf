variable "aws_region" {
  type        = string
  description = "The name of the AWS region"
}

variable "vpc_id" {
  type = string
}

variable "private_subnets" {
  type = list(any)
}

variable "public_subnets" {
  type = list(any)
}

variable "port" {
  type        = number
  description = "The port that the application runs on"
}

variable "fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the service"
}
variable "fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the service"
}

variable "image_url" {
  type        = string
  description = "String of backoffice image in ecr repository"
}

variable "execution_role_arn" {
  type        = string
  description = "IAM Execution Role Arn"
}

variable "task_role_arn" {
  type        = string
  description = "IAM Task Role Arn"
}

variable "tg_arn" {
  type        = string
  description = "Target group Arn"
}

variable "security_groups" {
  type        = list(any)
  description = "Security groups for ECS"
}

variable "environment_file" {
  sensitive   = true
  type        = string
  description = "The environment file for the ECS container"
}

variable "droits_ecs_cluster" {
  type        = string
  description = "The id of the ecs cluster"
}

variable "service_name" {
  type        = string
  description = "Name of the ecs service"
}

variable "health_check_url" {
  type        = string
  description = "Url of health check target"
}

variable "desired_count" {
  type        = number
  description = "Number of ecs services to provision"
  default     = 1
}

variable "health_check_grace_period" {
  type        = number
  description = "Time in seconds between health checks"
  default     = 600
}
