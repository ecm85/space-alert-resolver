resource "aws_iam_role_policy_attachment" "lambda_role_policy_attachment" {
  policy_arn = "arn:aws:iam::854713338508:policy/service-role/AWSLambdaBasicExecutionRole-32dfd47a-f0a6-46db-8768-bacf71fb39ec"
  role       = "space-alert-resolver-role-th1q8vfz"
}
