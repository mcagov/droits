output "backoffice_alerts_topic_arn" {
  value = aws_sns_topic.backoffice_alerts.arn
}
output "webapp_alerts_topic_arn" {
  value = aws_sns_topic.webapp_alerts.arn
}
output "db_alerts_topic_arn" {
  value = aws_sns_topic.db_alerts.arn
}