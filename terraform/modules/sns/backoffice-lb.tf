resource "aws_sns_topic_subscription" "backoffice_lb_alerts" {
  topic_arn = aws_sns_topic.backoffice_lb_alerts.arn
  protocol  = "email"
  endpoint  = var.alert_email_address
}

resource "aws_sns_topic" "backoffice_lb_alerts" {
  name = "${terraform.workspace}-backoffice-lb-alerts"
  tags = {
    Environment = terraform.workspace
    Application = "droits-backoffice-lb"
  }
}

resource "aws_sns_topic_policy" "backoffice_lb_alerts" {
  arn    = aws_sns_topic.backoffice_lb_alerts.arn
  policy = data.aws_iam_policy_document.backoffice_lb_alerts.json
}

data "aws_iam_policy_document" "backoffice_lb_alerts" {
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
      aws_sns_topic.backoffice_lb_alerts.arn
    ]

    sid = "__default_statement_ID"
  }
}