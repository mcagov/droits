resource "aws_cloudwatch_metric_alarm" "ecs_low_cpu_utilisation" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.service_name}-low-cpu-utilisation"
  comparison_operator = "LessThanThreshold"
  evaluation_periods  = var.cpu_utilization_low_evaluation_periods
  metric_name         = "CPUUtilization"
  namespace           = "AWS/ECS"
  period              = var.cpu_utilization_low_period
  statistic           = "Maximum"
  threshold           = var.cpu_utilization_low_threshold_percentage
  alarm_description   = "Low CPU Utilisation"
  alarm_actions       = var.enable_alerts == true ? [var.alerts_topic_arn] : []
  ok_actions          = var.enable_alerts == true ? [var.alerts_topic_arn] : []

  dimensions = {
    ClusterName = var.cluster_name
    ServiceName = var.service_name
  }
}

resource "aws_cloudwatch_metric_alarm" "ecs_high_cpu_utilisation" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.service_name}-high-cpu-utilisation"
  comparison_operator = "GreaterThanThreshold"
  evaluation_periods  = var.cpu_utilization_high_evaluation_periods
  metric_name         = "CPUUtilization"
  namespace           = "AWS/ECS"
  period              = var.cpu_utilization_high_period
  statistic           = "Maximum"
  threshold           = var.cpu_utilization_high_threshold_percentage
  alarm_description   = "High CPU Utilisation"
  alarm_actions       = var.enable_alerts == true ? [var.alerts_topic_arn] : []
  ok_actions          = var.enable_alerts == true ? [var.alerts_topic_arn] : []

  dimensions = {
    ClusterName = var.cluster_name
    ServiceName = var.service_name
  }
}

resource "aws_cloudwatch_metric_alarm" "ecs_low_memory_utilisation" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.service_name}-low-memory-utilisation"
  comparison_operator = "LessThanThreshold"
  evaluation_periods  = var.memory_utilization_low_evaluation_periods
  metric_name         = "MemoryUtilization"
  namespace           = "AWS/ECS"
  period              = var.memory_utilization_low_period
  statistic           = "Maximum"
  threshold           = var.memory_utilization_low_threshold_percentage
  alarm_description   = "Low Memory Utilisation"
  alarm_actions       = var.enable_alerts == true ? [var.alerts_topic_arn] : []
  ok_actions          = var.enable_alerts == true ? [var.alerts_topic_arn] : []

  dimensions = {
    ClusterName = var.cluster_name
    ServiceName = var.service_name
  }
}

resource "aws_cloudwatch_metric_alarm" "ecs_high_memory_utilisation" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.service_name}-high-memory-utilisation"
  comparison_operator = "GreaterThanThreshold"
  evaluation_periods  = var.memory_utilization_high_evaluation_periods
  metric_name         = "MemoryUtilization"
  namespace           = "AWS/ECS"
  period              = var.memory_utilization_high_period
  statistic           = "Maximum"
  threshold           = var.memory_utilization_high_threshold_percentage
  alarm_description   = "High Memory Utilisation"
  alarm_actions       = var.enable_alerts == true ? [var.alerts_topic_arn] : []
  ok_actions          = var.enable_alerts == true ? [var.alerts_topic_arn] : []

  dimensions = {
    ClusterName = var.cluster_name
    ServiceName = var.service_name
  }
}

resource "aws_cloudwatch_metric_alarm" "ecs_low_task_count" {
  alarm_name          = "ecs-${terraform.workspace}-droits-${var.service_name}-low-task-count"
  comparison_operator = "LessThanThreshold"
  evaluation_periods  = var.task_count_low_evaluation_periods
  metric_name         = "RunningTaskCount"
  namespace           = "AWS/ECS"
  period              = var.task_count_low_period
  statistic           = "Average"
  threshold           = var.minimum_task_count
  alarm_description   = "Task count is too low."
  alarm_actions       = var.enable_alerts == true ? [var.alerts_topic_arn] : []
  ok_actions          = var.enable_alerts == true ? [var.alerts_topic_arn] : []

  dimensions = {
    ClusterName = var.cluster_name
    ServiceName = var.service_name
  }
}
