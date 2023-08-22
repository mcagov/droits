resource "aws_iam_policy" "ecs_s3_access" {
  name        = "ECSBackofficeS3Access"
  description = "Grant ECS task access to specific S3 bucket operations"

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action   = ["s3:ListBucket"],
        Effect   = "Allow",
        Resource = [module.s3-images.bucket_arn]
      },
      {
        Action   = ["s3:PutObject", "s3:GetObject", "s3:DeleteObject"],
        Effect   = "Allow",
        Resource = ["${module.s3-images.bucket_arn}/*"]
      }
    ]
  })
}


resource "aws_iam_role_policy_attachment" "ecs_s3_access_attach" {
  role       = module.iam.iam_role_name
  policy_arn = aws_iam_policy.ecs_s3_access.arn
}

