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
  description = "The port that the backoffice application runs on"
}

variable "fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the DROITS backoffice service"
}
variable "fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the DROITS backoffice service"
}

variable "image_url" {
  type        = string
  description = "String of backoffice image in ecr repository"
}

variable "iam_role_arn" {
  type        = string
  description = "IAM Role Arn"
}

variable "tg_arn" {
  type        = string
  description = "Backoffice target group Arn"
}

variable "security_groups" {
  type        = list(any)
  description = "Security groups for Backoffice ECS"
}

variable "environment_file" {
  sensitive   = true
  type        = string
  description = "The environment file for the backoffice ECS container"
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
