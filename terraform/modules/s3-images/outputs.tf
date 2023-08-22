output "bucket_arn" {
  description = "The ARN of the S3 bucket"
  value       = "arn:aws:s3:::${aws_s3_bucket.droits-wreck-images.bucket}"
}
