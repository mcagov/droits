resource "aws_cloudwatch_log_group" "droits-backoffice-container-logs" {
  name              = "droits-backoffice-container-logs"
  retention_in_days = 30

  tags = {
    Environment = terraform.workspace
    Application = "droits-backoffice"
  }
}

resource "aws_cloudwatch_log_stream" "backoffice" {
  name           = "droits-backoffice-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.droits-backoffice-container-logs.name
}


resource "aws_cloudwatch_log_group" "droits-webapp-container-logs" {
  name              = "droits-webapp-container-logs"
  retention_in_days = 30
  tags = {
    Environment = terraform.workspace
    Application = "droits-webapp"
  }
}

resource "aws_cloudwatch_log_stream" "webapp" {
  name           = "droits-webapp-ecs-stream"
  log_group_name = aws_cloudwatch_log_group.droits-webapp-container-logs.name
}