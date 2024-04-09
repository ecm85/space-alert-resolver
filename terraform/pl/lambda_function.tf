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
  runtime                        = "dotnet6"
  filename                       = "pl-lambda.zip"
  source_code_hash               = filebase64sha256("pl-lambda.zip")
  timeout                        = "15"

  tracing_config {
    mode = "PassThrough"
  }
}

resource "aws_lambda_permission" "allow_api_gateway_1" {
  statement_id  = "AllowSpaceAlertExecutionFromApiGateway1"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.lambda.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_api_gateway_rest_api.rest_api.execution_arn}/*/*/"
}

resource "aws_lambda_permission" "allow_api_gateway_2" {
  statement_id  = "AllowSpaceAlertExecutionFromApiGateway2"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.lambda.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_api_gateway_rest_api.rest_api.execution_arn}/*/*/*"
}
