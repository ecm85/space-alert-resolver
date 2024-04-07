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

  managed_policy_arns  = ["arn:aws:iam::854713338508:policy/service-role/AWSLambdaBasicExecutionRole-32dfd47a-f0a6-46db-8768-bacf71fb39ec"]
  max_session_duration = "3600"
  name                 = "space-alert-resolver-role-th1q8vfz"
  path                 = "/service-role/"
}
