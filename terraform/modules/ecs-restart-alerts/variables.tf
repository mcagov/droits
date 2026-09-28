variable "service_name" {
  type        = string
  description = "The name of the ECS service to watch for container restarts. E.g webapp"
}

variable "cluster_arn" {
  type        = string
  description = "The ARN of the ECS cluster the service runs in"
}

variable "alerts_topic_arn" {
  type        = string
  description = "The ARN of the SNS topic that forwards alerts to PagerDuty"
}
