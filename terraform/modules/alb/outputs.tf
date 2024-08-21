output "target-group-arn" {
  value = aws_alb_target_group.target-group.arn
}
output "alb-name" {
  value = aws_alb.alb.name
}
output "alb-id" {
  value = aws_alb.alb.id
}
output "alb-arn-suffix" {
  value = aws_alb.alb.arn_suffix
}
output "target-group-id" {
  value = aws_alb_target_group.target-group.id
}
output "alb-dns" {
  value = {
    zone_id : aws_alb.alb.zone_id,
    dns_name : aws_alb.alb.dns_name
  }
}