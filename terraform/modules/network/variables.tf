variable "aws_vpc_id" {
  type        = string
  description = "ID of main droits vpc"
}
variable "private_subnet_1" {
  type        = string
  description = "ID of first private subnet"
}
variable "private_subnet_2" {
  type        = string
  description = "ID of second private subnet"
}
variable "public_subnet_1" {
  type        = string
  description = "ID of first public subnet"
}
variable "public_subnet_2" {
  type        = string
  description = "ID of second public subnet"
}