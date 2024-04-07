resource "aws_iam_policy" "lambda_execution_policy" {
  name = "AWSLambdaBasicExecutionRole-32dfd47a-f0a6-46db-8768-bacf71fb39ec"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:us-east-2:854713338508:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:us-east-2:854713338508:log-group:/aws/lambda/space-alert-resolver:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}
