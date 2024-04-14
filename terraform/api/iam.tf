resource "aws_iam_policy" "lambda_execution_policy" {
  name = "space-alert-api-lambda-execution-role"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents",
        "ses: SendEmail"
      ],
      "Effect": "Allow",
      "Resource": ["*"]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_role" "lambda_role" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  max_session_duration = "3600"
  name                 = "space-alert-api-role"
  path                 = "/service-role/"
}

resource "aws_iam_role_policy_attachment" "lambda_role_policy_attachment" {
  policy_arn = aws_iam_policy.lambda_execution_policy.arn
  role       = aws_iam_role.lambda_role.name
}
