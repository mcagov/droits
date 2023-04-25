variable "aws_region" {
  type        = string
  description = "The name of the AWS region"
}

variable "ecs_cluster_name" {
  type        = string
  description = "The name of the ECS cluster that houses all our services"
}

variable "backoffice_port" {
  type        = string
  description = "The port that the backoffice application runs on"
}

variable "webapp_port" {
  type        = string
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
