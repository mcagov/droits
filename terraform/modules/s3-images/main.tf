resource "aws_s3_bucket" "droits-wreck-images" {
  bucket = "droits-wreck-images-${terraform.workspace}"
  # Stops terraform from destroying the object if it exists
  lifecycle {
    prevent_destroy = false
  }
}

#resource "aws_s3_bucket_acl" "droits-wreck-images-acl" {
#  bucket = "droits-wreck-images-${terraform.workspace}"
#  acl    = "private"
#}

resource "aws_s3_bucket_versioning" "droits-wreck-images-versioning" {
  bucket = aws_s3_bucket.droits-wreck-images.bucket
  versioning_configuration {
    status = "Enabled"
  }
  depends_on = [aws_s3_bucket.droits-wreck-images]
}

resource "aws_s3_bucket_server_side_encryption_configuration" "droits-wreck-images-encryption-config" {
  bucket = aws_s3_bucket.droits-wreck-images.bucket
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
  depends_on = [aws_s3_bucket.droits-wreck-images]
}
