terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.27.0"
    }
  }

  backend "s3" {
    bucket = "droits-statefile"
    key = "global/s3/terraform.tfstate"
    encrypt = true
    region = "eu-west-2"
    # Variables are not allowed here otherwise I'd have specified the access key here
  }
}

# Configure the AWS Provider
provider "aws" {
  region = var.aws_region
  access_key = var.aws_access_key_id
  secret_key = var.aws_secret_access_key
  session_token = var.aws_session_token
}

resource "aws_s3_bucket" "droits-images"{
    bucket = "droits-images"
    # Stops terraform from destroying the object if it exists
    lifecycle {
      prevent_destroy = true
    }
}

resource "aws_s3_bucket_acl" "droits-images-acl" {
  bucket = "droits-images"
  acl    = "private"
}

resource "aws_s3_bucket_versioning" "droits-images-versioning" {
  bucket = "droits-images"
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "droits-images-encryption-config" {
  bucket = "droits-images"
  rule {
        apply_server_side_encryption_by_default {
            sse_algorithm = "AES256"
        }
      }
}