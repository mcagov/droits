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
}

resource "aws_s3_bucket" "droits-wreck-images"{
    bucket = "droits-wreck-images"
    # Stops terraform from destroying the object if it exists
    lifecycle {
      prevent_destroy = true
    }
}

resource "aws_s3_bucket_acl" "droits-wreck-images-acl" {
  bucket = "droits-wreck-images"
  acl    = "private"
}

resource "aws_s3_bucket_versioning" "droits-wreck-images-versioning" {
  bucket = "droits-wreck-images"
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "droits-wreck-images-encryption-config" {
  bucket = "droits-wreck-images"
  rule {
        apply_server_side_encryption_by_default {
            sse_algorithm = "AES256"
        }
      }
}

resource "aws_ecr_repository" "droits-webapp-repository" {
  name         = var.webapp_ecr_repository_name
  force_delete = true
}

resource "aws_ecr_repository" "droits-api-backoffice-repository" {
  name         = var.api_backoffice_ecr_repository_name
  force_delete = true
}

resource "aws_ecs_cluster" "droits-ecs-cluster" {
  name = var.ecs_cluster_name
}

resource "aws_db_instance" "droits_db" {
  allocated_storage    = var.db_allocated_storage
  db_name              = var.db_name
  identifier           = "${terraform.workspace}-droits-database"
  engine               = "pstgres"
  engine_version       = "14"
  instance_class       = "db.t3.micro"
  username             = var.db_username
  password             = var.db_password
  db_subnet_group_name            = aws_db_subnet_group.db.id
  vpc_security_group_ids          = [aws_security_group.db.id]
  deletion_protection             = var.db_delete_protection
  parameter_group_name = "default.postgres14"
  skip_final_snapshot  = true
  storage_encrypted = true
}