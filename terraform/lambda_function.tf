resource "aws_lambda_function" "tfer--space-002D-alert-002D-resolver" {
  architectures = ["x86_64"]

  ephemeral_storage {
    size = "512"
  }

  function_name                  = "space-alert-resolver"
  handler                        = "PL::PL.LambdaEntryPoint::FunctionHandlerAsync"
  memory_size                    = "2048"
  package_type                   = "Zip"
  reserved_concurrent_executions = "-1"
  role                           = "arn:aws:iam::854713338508:role/service-role/space-alert-resolver-role-th1q8vfz"
  runtime                        = "dotnetcore3.1"
  source_code_hash               = filebase64sha256("lambda.zip")
  timeout                        = "15"

  tracing_config {
    mode = "PassThrough"
  }
}
