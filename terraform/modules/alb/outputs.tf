output "webapp-target-group-arn" {
  value = aws_alb_target_group.webapp-target-group.arn
}
output "api-backoffice-target-group-arn" {
  value = aws_alb_target_group.api-backoffice-target-group.arn
}
output "backoffice-alb-name" {
  value = aws_alb.api-backoffice-alb.name
}
output "backoffice-alb-id" {
  value = aws_alb.api-backoffice-alb.id
}
output "backoffice-alb-arn-suffix" {
  value = aws_alb.api-backoffice-alb.arn_suffix
}
output "backoffice-target-group-id" {
  value = aws_alb_target_group.api-backoffice-target-group.id
}
output "webapp-alb-name" {
  value = aws_alb.webapp-alb.name
}
output "webapp-alb-id" {
  value = aws_alb.webapp-alb.id
}
output "webapp-alb-arn-suffix" {
  value = aws_alb.webapp-alb.arn_suffix
}
output "webapp-target-group-id" {
  value = aws_alb_target_group.webapp-target-group.id
}