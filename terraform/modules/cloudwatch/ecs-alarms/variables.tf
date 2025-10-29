variable "cluster_name" {
  type = string
}
variable "service_name" {
  type = string
}
variable "alerts_topic_arn" {
  type        = string
  description = "The ARN of the backoffice_alerts SNS topic"
}
variable "enable_alerts" {
  type        = bool
  description = "When enabled CloudWatch alarm events are sent to the Alerts SNS Topic"
}

# ECS Task Count
# Todo: Can we just use the minimum task count from ECS as the threshold?
variable "minimum_task_count" {
  type        = number
  description = "Minimum number of expected tasks to be running for the backoffice ECS service"
  default     = 1
}
variable "task_count_low_evaluation_periods" {
  type        = number
  description = "Number of periods to evaluate for the alarm"
  default     = 1
}
variable "task_count_low_period" {
  type        = number
  description = "Duration in seconds to evaluate for the alarm"
  default     = 300
}

# ECS high CPU Utilisation
variable "cpu_utilization_high_threshold_percentage" {
  type        = number
  description = "The % CPU utilisation limit which, when passed, will trigger an alarm. This will be higher for dev and lower for production."
  default     = 90
}
variable "cpu_utilization_high_evaluation_periods" {
  type        = number
  description = "Number of periods to evaluate for the alarm"
  default     = 1
}
variable "cpu_utilization_high_period" {
  type        = number
  description = "Duration in seconds to evaluate for the alarm"
  default     = 300
}

# ECS low CPU Utilisation
variable "cpu_utilization_low_threshold_percentage" {
  type        = number
  description = "The % CPU utilisation limit which, when consistently under, will trigger an alarm."
  default     = 20
}
variable "cpu_utilization_low_evaluation_periods" {
  type        = number
  description = "Number of periods to evaluate for the alarm"
  default     = 7 # 7 days
}
variable "cpu_utilization_low_period" {
  type        = number
  description = "Duration in seconds to evaluate for the alarm"
  default     = 86400 # 1 day
}

# ECS high memory utilisation
variable "memory_utilization_high_threshold_percentage" {
  type        = number
  description = "The % CPU utilisation limit which, when passed, will trigger an alarm. This will be higher for dev and lower for production."
  default     = 90
}
variable "memory_utilization_high_evaluation_periods" {
  type        = number
  description = "Number of periods to evaluate for the alarm"
  default     = 1
}
variable "memory_utilization_high_period" {
  type        = number
  description = "Duration in seconds to evaluate for the alarm"
  default     = 300
}

# ECS low memory utilisation
variable "memory_utilization_low_threshold_percentage" {
  type        = number
  description = "The % CPU utilisation limit which, when consistently under, will trigger an alarm."
  default     = 10
}
variable "memory_utilization_low_evaluation_periods" {
  type        = number
  description = "Number of periods to evaluate for the alarm"
  default     = 7 # 7 days
}
variable "memory_utilization_low_period" {
  type        = number
  description = "Duration in seconds to evaluate for the alarm"
  default     = 86400 # 1 day
}

