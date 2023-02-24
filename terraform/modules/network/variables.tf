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
variable "public_subnet_1" {
  type        = string
  description = "ID of first public subnet"
  default     = "subnet-0acd501299e0d6d8a"
}
variable "public_subnet_2" {
  type        = string
  description = "ID of second public subnet"
  default     = "subnet-014306c736144d6f9"
}