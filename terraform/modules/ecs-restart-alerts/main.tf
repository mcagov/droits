locals {
  alarm_name = "ecs-${terraform.workspace}-droits-${var.service_name}-container-restarted"
}

resource "aws_cloudwatch_event_rule" "task_stopped" {
  name        = "ecs-${terraform.workspace}-droits-${var.service_name}-task-stopped"
  description = "ECS container stopped"

  event_pattern = jsonencode({
    source        = ["aws.ecs"]
    "detail-type" = ["ECS Task State Change"]
    detail = {
      clusterArn = [var.cluster_arn]
      group      = ["service:${var.service_name}"]
      lastStatus = ["STOPPED"]
    }
  })
}

resource "aws_cloudwatch_event_target" "task_stopped" {
  rule      = aws_cloudwatch_event_rule.task_stopped.name
  target_id = "${var.service_name}-task-stopped-alerts"
  arn       = var.alerts_topic_arn

  input_transformer {
    input_paths = {
      time     = "$.time"
      account  = "$.account"
      region   = "$.region"
      taskArn  = "$.detail.taskArn"
      stopCode = "$.detail.stopCode"
      reason   = "$.detail.stoppedReason"
    }

    input_template = <<-EOT
      {
        "AlarmName": "${local.alarm_name}",
        "AlarmDescription": "Container has restarted",
        "NewStateValue": "ALARM",
        "NewStateReason": "Task <taskArn> stopped with code <stopCode>: <reason>",
        "StateChangeTime": "<time>",
        "Region": "<region>",
        "AWSAccountId": "<account>"
      }
    EOT
  }
}

resource "aws_cloudwatch_event_rule" "task_running" {
  name        = "ecs-${terraform.workspace}-droits-${var.service_name}-task-running"
  description = "ECS container running"

  event_pattern = jsonencode({
    source        = ["aws.ecs"]
    "detail-type" = ["ECS Task State Change"]
    detail = {
      clusterArn    = [var.cluster_arn]
      group         = ["service:${var.service_name}"]
      lastStatus    = ["RUNNING"]
      desiredStatus = ["RUNNING"]
    }
  })
}

resource "aws_cloudwatch_event_target" "task_running" {
  rule      = aws_cloudwatch_event_rule.task_running.name
  target_id = "${var.service_name}-task-running-alerts"
  arn       = var.alerts_topic_arn

  input_transformer {
    input_paths = {
      time    = "$.time"
      account = "$.account"
      region  = "$.region"
      taskArn = "$.detail.taskArn"
    }

    input_template = <<-EOT
      {
        "AlarmName": "${local.alarm_name}",
        "AlarmDescription": "Container has restarted",
        "NewStateValue": "OK",
        "NewStateReason": "Task <taskArn> is running.",
        "StateChangeTime": "<time>",
        "Region": "<region>",
        "AWSAccountId": "<account>"
      }
    EOT
  }
}
