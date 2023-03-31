output "backoffice-alerts-topic-arn" {
  value = aws_sns_topic.backoffice_alerts.arn
}
output "webapp-alerts-topic-arn" {
  value = aws_sns_topic.webapp_alerts.arn
}
output "db-alerts-topic-arn" {
  value = aws_sns_topic.db_alerts.arn
}
output "backoffice-lb-alerts-topic-arn" {
  value = aws_sns_topic.backoffice_lb_alerts.arn
}
output "webapp-lb-alerts-topic-arn" {
  value = aws_sns_topic.webapp_lb_alerts.arn
}