
resource "aws_s3_bucket" "alb-logs" {
  bucket = "droits-${var.application_name}-alb-logs-${terraform.workspace}"
  # Stops terraform from destroying the object if it exists
  lifecycle {
    prevent_destroy = false
  }
}

#resource "aws_s3_bucket_acl" "alb-logs" {
#  bucket = aws_s3_bucket.alb-logs.bucket
#  acl    = "log-delivery-write"
#
#  depends_on = [aws_s3_bucket.alb-logs, aws_s3_bucket_policy.alb-logs]
#}

resource "aws_s3_bucket_versioning" "alb-logs" {
  bucket = aws_s3_bucket.alb-logs.bucket
  versioning_configuration {
    status = "Enabled"
  }
  depends_on = [aws_s3_bucket.alb-logs]
}

resource "aws_s3_bucket_server_side_encryption_configuration" "alb-logs" {
  bucket = aws_s3_bucket.alb-logs.bucket
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
  depends_on = [aws_s3_bucket.alb-logs]
}

resource "aws_s3_bucket_policy" "alb-logs" {
  bucket = aws_s3_bucket.alb-logs.bucket

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
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.alb-logs.bucket}/*"
        },
        {
          "Sid" : "AWSLogDeliveryWrite",
          "Effect" : "Allow",
          "Principal" : {
            "Service" : "delivery.logs.amazonaws.com"
          },
          "Action" : "s3:PutObject",
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.alb-logs.bucket}/*",
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
          "Resource" : "arn:aws:s3:::${aws_s3_bucket.alb-logs.bucket}"
        }
      ]
  })
  depends_on = [aws_s3_bucket.alb-logs]
}
