terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.1"
    }
  }

  # Requires S3 bucket & Dynamo DB to be configured, please see README.md
  backend "s3" {
    bucket         = "droits-terraform-statefile"
    encrypt        = true
    region         = "eu-west-2"
  }

  required_version = "~>1.0.0"
}