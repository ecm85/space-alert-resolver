resource "aws_lambda_function" "lambda" {
  architectures = ["x86_64"]

  ephemeral_storage {
    size = "512"
  }

  function_name                  = "space-alert-resolver"
  handler                        = "PL::PL.LambdaEntryPoint::FunctionHandlerAsync"
  memory_size                    = "2048"
  package_type                   = "Zip"
  reserved_concurrent_executions = "-1"
  role                           = aws_iam_role.lambda_role.arn
  runtime                        = "dotnet6.0"
  filename                       = "lambda.zip"
  source_code_hash               = filebase64sha256("lambda.zip")
  timeout                        = "15"

  tracing_config {
    mode = "PassThrough"
  }
}
