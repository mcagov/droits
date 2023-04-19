resource "aws_s3_bucket" "droits-wreck-images" {
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

resource "aws_s3_bucket" "droits-backoffice-alb-logs" {
  bucket = "droits-backoffice-alb-logs"
  # Stops terraform from destroying the object if it exists
  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_s3_bucket_acl" "droits-backoffice-alb-logs" {
  bucket = "droits-backoffice-alb-logs"
  acl    = "log-delivery-write"
}

resource "aws_s3_bucket_versioning" "droits-backoffice-alb-logs" {
  bucket = "droits-backoffice-alb-logs"
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "droits-backoffice-alb-logs" {
  bucket = "droits-backoffice-alb-logs"
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_policy" "droits-backoffice-alb-logs" {
  bucket = "droits-backoffice-alb-logs"

  policy = jsonencode(
    {
      "Version" : "2012-10-17",
      "Statement" : [
        {
          "Sid" : "AllowELBRootAccount",
          "Effect" : "Allow",
          "Principal" : {
            "AWS" : "arn:aws:iam::${var.regional_account_id}:root"
          },
          "Action" : "s3:PutObject",
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.droits-backoffice-alb-logs.bucket}/*"
        },
        {
          "Sid" : "AWSLogDeliveryWrite",
          "Effect" : "Allow",
          "Principal" : {
            "Service" : "delivery.logs.amazonaws.com"
          },
          "Action" : "s3:PutObject",
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.droits-backoffice-alb-logs.bucket}/*",
          "Condition" : {
            "StringEquals" : {
              "s3:x-amz-acl" : "bucket-owner-full-control"
            }
          }
        },
        {
          "Sid" : "AWSLogDeliveryAclCheck",
          "Effect" : "Allow",
          "Principal" : {
            "Service" : "delivery.logs.amazonaws.com"
          },
          "Action" : "s3:GetBucketAcl",
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.droits-backoffice-alb-logs.bucket}"
        }
      ]
  })
}

resource "aws_s3_bucket" "droits-webapp-alb-logs" {
  bucket = "droits-webapp-alb-logs"
  # Stops terraform from destroying the object if it exists
  lifecycle {
    prevent_destroy = true
  }
}

resource "aws_s3_bucket_acl" "droits-webapp-alb-logs" {
  bucket = "droits-webapp-alb-logs"
  acl    = "log-delivery-write"
}

resource "aws_s3_bucket_versioning" "droits-webapp-alb-logs" {
  bucket = "droits-webapp-alb-logs"
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "droits-webapp-alb-logs" {
  bucket = "droits-webapp-alb-logs"
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_policy" "droits-webapp-alb-logs" {
  bucket = "droits-webapp-alb-logs"

  policy = jsonencode(
    {
      "Version" : "2012-10-17",
      "Statement" : [
        {
          "Sid" : "AllowELBRootAccount",
          "Effect" : "Allow",
          "Principal" : {
            "AWS" : "arn:aws:iam::${var.regional_account_id}:root"
          },
          "Action" : "s3:PutObject",
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.droits-webapp-alb-logs.bucket}/*"
        },
        {
          "Sid" : "AWSLogDeliveryWrite",
          "Effect" : "Allow",
          "Principal" : {
            "Service" : "delivery.logs.amazonaws.com"
          },
          "Action" : "s3:PutObject",
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.droits-webapp-alb-logs.bucket}/*",
          "Condition" : {
            "StringEquals" : {
              "s3:x-amz-acl" : "bucket-owner-full-control"
            }
          }
        },
        {
          "Sid" : "AWSLogDeliveryAclCheck",
          "Effect" : "Allow",
          "Principal" : {
            "Service" : "delivery.logs.amazonaws.com"
          },
          "Action" : "s3:GetBucketAcl",
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.droits-webapp-alb-logs.bucket}"
        }
      ]
  })
}