resource "aws_sns_topic" "alerts" {
  name = "${terraform.workspace}-${var.resource_name}-alerts"
  tags = {
    Environment = terraform.workspace
    Application = "droits-${var.resource_name}"
  }
}

resource "aws_sns_topic_subscription" "alerts" {
  topic_arn = aws_sns_topic.alerts.arn
  protocol  = "email"
  endpoint  = var.alert_email_address

  depends_on = [aws_sns_topic.alerts]
}

resource "aws_sns_topic_subscription" "sns_technical_alerts_pagerduty_subscription" {
  topic_arn = aws_sns_topic.alerts.arn
  protocol  = "https"
  endpoint  = var.alert_pagerduty_integration_url
}


resource "aws_sns_topic_policy" "alerts" {
  arn    = aws_sns_topic.alerts.arn
  policy = data.aws_iam_policy_document.alerts.json

  depends_on = [aws_sns_topic.alerts]
}

data "aws_iam_policy_document" "alerts" {
  policy_id = "__default_policy_ID"

  statement {
    actions = [
      "SNS:Subscribe",
      "SNS:SetTopicAttributes",
      "SNS:RemovePermission",
      "SNS:Receive",
      "SNS:Publish",
      "SNS:ListSubscriptionsByTopic",
      "SNS:GetTopicAttributes",
      "SNS:DeleteTopic",
      "SNS:AddPermission",
    ]

    condition {
      test     = "StringEquals"
      variable = "AWS:SourceOwner"

      values = [
        var.aws_account_number,
      ]
    }

    effect = "Allow"

    principals {
      type        = "AWS"
      identifiers = ["*"]
    }

    resources = [
      aws_sns_topic.alerts.arn
    ]

    sid = "__default_statement_ID"
  }

  depends_on = [aws_sns_topic.alerts]
}
