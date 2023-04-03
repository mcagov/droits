variable "resource_name" {
  type        = string
  description = "The name of the resource that the SNS topic relates to. E.g webapp-lb"
}

variable "alert_email_address" {
  sensitive   = true
  type        = string
  description = "Email Address subscribed to alerts"
}
variable "aws_account_number" {
  sensitive   = true
  type        = string
  description = "The MCA's AWS account number"
}