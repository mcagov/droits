output "backoffice-lb-log-bucket" {
  value = aws_s3_bucket.droits-backoffice-alb-logs.bucket
}
output "webapp-lb-log-bucket" {
  value = aws_s3_bucket.droits-webapp-alb-logs.bucket
}