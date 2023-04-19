output "cluster-name" {
  value = aws_ecs_cluster.droits-ecs-cluster.name
}
output "backoffice-service-name" {
  value = aws_ecs_service.backoffice-service.name
}
output "webapp-service-name" {
  value = aws_ecs_service.webapp.name
}
