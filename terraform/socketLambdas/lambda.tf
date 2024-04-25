resource "aws_lambda_function" "create-game-lambda" {
  architectures = ["x86_64"]

  ephemeral_storage {
    size = "512"
  }

  function_name                  = "create-game-lambda"
  handler                        = "CreateGameLambda::CreateGameLambda.Function::FunctionHandler"
  memory_size                    = "2048"
  package_type                   = "Zip"
  reserved_concurrent_executions = "-1"
  role                           = aws_iam_role.lambda_role.arn
  runtime                        = "dotnet6"
  filename                       = "create-game-lambda.zip"
  source_code_hash               = filebase64sha256("create-game-lambda.zip")
  timeout                        = "15"

  tracing_config {
    mode = "PassThrough"
  }
}

resource "aws_lambda_permission" "allow_api_gateway_create_game_1" {
  statement_id  = "AllowCreateGameExecutionFromApiGateway1"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.create-game-lambda.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_apigatewayv2_api.sockets_gateway.execution_arn}/*/$default"
}

resource "aws_lambda_permission" "allow_api_gateway_create_game_2" {
  statement_id  = "AllowCreateGameExecutionFromApiGateway2"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.create-game-lambda.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_apigatewayv2_api.sockets_gateway.execution_arn}/*/CreateGame"
}

resource "aws_lambda_function" "join-game-lambda" {
  architectures = ["x86_64"]

  ephemeral_storage {
    size = "512"
  }

  function_name                  = "join-game-lambda"
  handler                        = "JoinGameLambda::JoinGameLambda.Function::FunctionHandler"
  memory_size                    = "2048"
  package_type                   = "Zip"
  reserved_concurrent_executions = "-1"
  role                           = aws_iam_role.lambda_role.arn
  runtime                        = "dotnet6"
  filename                       = "join-game-lambda.zip"
  source_code_hash               = filebase64sha256("join-game-lambda.zip")
  timeout                        = "15"

  tracing_config {
    mode = "PassThrough"
  }
}

resource "aws_lambda_permission" "allow_api_gateway_join_game" {
  statement_id  = "AllowJoinGameExecutionFromApiGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.join-game-lambda.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_apigatewayv2_api.sockets_gateway.execution_arn}/*/JoinGame"
}

resource "aws_lambda_function" "send-to-host-lambda" {
  architectures = ["x86_64"]

  ephemeral_storage {
    size = "512"
  }

  function_name                  = "send-to-host-lambda"
  handler                        = "SendToHostLambda::SendToHostLambda.Function::FunctionHandler"
  memory_size                    = "2048"
  package_type                   = "Zip"
  reserved_concurrent_executions = "-1"
  role                           = aws_iam_role.lambda_role.arn
  runtime                        = "dotnet6"
  filename                       = "send-to-host-lambda.zip"
  source_code_hash               = filebase64sha256("send-to-host-lambda.zip")
  timeout                        = "15"

  tracing_config {
    mode = "PassThrough"
  }
}

resource "aws_lambda_permission" "allow_api_gateway_send_to_host" {
  statement_id  = "AllowSendToHostExecutionFromApiGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.send-to-host-lambda.function_name
  principal     = "apigateway.amazonaws.com"
  source_arn    = "${aws_apigatewayv2_api.sockets_gateway.execution_arn}/*/SendToHost"
}
