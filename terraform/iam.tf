resource "aws_iam_policy" "lambda_execution_policy" {
  name = "AWSLambdaBasicExecutionRole-lambda-execution-role"
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

  managed_policy_arns  = ["arn:aws:iam::854713338508:policy/service-role/AWSLambdaBasicExecutionRole-lambda-execution-role"]
  max_session_duration = "3600"
  name                 = "space-alert-resolver-role"
  path                 = "/service-role/"
}

resource "aws_iam_role_policy_attachment" "lambda_role_policy_attachment" {
  policy_arn = "arn:aws:iam::854713338508:policy/service-role/AWSLambdaBasicExecutionRole-lambda-execution-role"
  role       = "space-alert-resolver-role"
}
