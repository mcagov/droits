variable "ecs_cluster_name" {
  type        = string
  description = "The name of the ECS cluster that houses all our services"
}

variable "api_backoffice_port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "public_subnet_1" {
  type        = string
  description = "The name of the first public subnet"
}
variable "public_subnet_2" {
  type        = string
  description = "The name of the second public subnet"
}

variable "api-backoffice-lb-security-group-id" {
  type        = string
  description = "The ID of the security group in which the backoffice app's load balancer resides"
}

variable "webapp_port" {
  type        = string
  description = "The port that the webapp runs on"
}

variable "webapp-lb-security-group-id" {
  type        = string
  description = "The ID of the security group in which the webapp's load balancer resides"
}

variable "vpc_id" {
  type        = string
  description = "The ID of the main vpc"
}

variable "backoffice_security_group" {
  type        = string
  description = "The ID of the backoffice security group"
}

variable "webapp_security_group" {
  type        = string
  description = "The ID of the webapp security group"
}

variable "execution_role_arn" {
  type        = string
  description = "IAM ID for execution role"
}

variable "webapp_fargate_cpu" {
  type        = number
  description = "Fargate instance CPU units to provision (1 vCPU = 1024 CPU units) for the DROITS Webapp"
}
variable "webapp_fargate_memory" {
  type        = number
  description = "Fargate instance memory to provision (in MiB) for the DROITS Webapp"
}
variable "webapp_container_cpu" {
  type        = number
  description = "Container instance CPU units to provision (1 vCPU = 1024 CPU units) for the DROITS Webapp"
}
variable "webapp_container_memory" {
  type        = number
  description = "Container instance memory to provision (in MiB) for the DROITS Webapp"
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
variable "backoffice_container_cpu" {
  type        = number
  description = "Container instance CPU units to provision (1 vCPU = 1024 CPU units) for the DROITS Webapp"
}
variable "backoffice_container_memory" {
  type        = number
  description = "Container instance memory to provision (in MiB) for the DROITS Webapp"
}

variable "backoffice_image_url" {
  type        = string
  description = "String of backoffice image in ecr repository"
}

variable "webapp_target_group_arn" {
  type        = string
  description = "ARN of webapp lb target group"
}

variable "api_backoffice_target_group_arn" {
  type        = string
  description = "ARN of backoffice lb target group"
}

