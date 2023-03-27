resource "aws_cloudwatch_log_group" "backoffice_ecs" {
  name = "droits-backoffice-ecs-lg"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_stream" "backoffice" {
  name           = "droits-backoffice-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.backoffice_ecs.name
}


resource "aws_cloudwatch_log_group" "webapp_ecs" {
  name = "droits-webapp-ecs-lg"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_stream" "webapp" {
  name           = "droits-webapp-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.webapp_ecs.name
}