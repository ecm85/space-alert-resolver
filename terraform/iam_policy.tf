resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-32dfd47a-002D-f0a6-002D-46db-002D-8768-002D-bacf71fb39ec" {
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
