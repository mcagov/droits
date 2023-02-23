variable "aws_vpc_id" {
  type        = string
  description = "ID of main droits vpc"
  default     = "vpc-052a083e9af2d7145"
}
variable "private_subnet_1" {
  type        = string
  description = "ID of first private subnet"
  default     = "subnet-07142266aa23c47b8"
}
variable "private_subnet_2" {
  type        = string
  description = "ID of second private subnet"
  default     = "subnet-06fa9c8603b7dc463"
}