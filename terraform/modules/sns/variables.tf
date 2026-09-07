variable "resource_name" {
  type        = string
  description = "The name of the resource that the SNS topic relates to. E.g webapp-lb"
}

variable "alert_email_address" {
  sensitive   = true
  type        = string
  description = "Email Address subscribed to alerts"
}
variable "alert_pagerduty_integration_url" {
  sensitive   = true
  type        = string
  description = " The Integration URL to which CloudWatch alerts should be sent for PagerDuty"
}
variable "aws_account_number" {
  sensitive   = true
  type        = string
  description = "The MCA's AWS account number"
}