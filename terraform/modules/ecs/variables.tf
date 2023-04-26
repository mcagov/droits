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

variable "backoffice_port" {
  type        = number
  description = "The port that the backoffice application runs on"
}

variable "webapp_port" {
  type        = number
  description = "The port that the webapp runs on"
}

variable "webapp_fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the DROITS Webapp"
}
variable "webapp_fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the DROITS Webapp"
}

variable "webapp_image_url" {
  type        = string
  description = "string of webapp image in ecr repository"
}

variable "backoffice_fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the DROITS backoffice service"
}
variable "backoffice_fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the DROITS backoffice service"
}

variable "backoffice_image_url" {
  type        = string
  description = "String of backoffice image in ecr repository"
}

variable "iam_role_arn" {
  type        = string
  description = "IAM Role Arn"
}

variable "backoffice_tg_arn" {
  type        = string
  description = "Backoffice target group Arn"
}

variable "webapp_tg_arn" {
  type        = string
  description = "Webapp target group Arn"
}

variable "backoffice_security_groups" {
  type        = list(any)
  description = "Security groups for Backoffice ECS"
}

variable "webapp_security_groups" {
  type        = list(any)
  description = "Security groups for Webapp ECS"
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

variable "droits_ecs_cluster" {
  type = string
  description = "The id of the ecs cluster"
}
