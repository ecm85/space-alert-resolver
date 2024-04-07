
resource "aws_api_gateway_integration" "gateway_integration" {
  cache_namespace         = aws_api_gateway_resource.gateway_resource.id
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = "ANY"
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  resource_id             = aws_api_gateway_resource.gateway_resource.id
  rest_api_id             = aws_api_gateway_rest_api.rest_api.id
  timeout_milliseconds    = "29000"
  type                    = "AWS_PROXY"
  uri                     = "arn:aws:apigateway:us-east-2:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-2:854713338508:function:space-alert-resolver/invocations"
}

resource "aws_api_gateway_integration" "proxy_gateway_integration" {
  cache_key_parameters    = ["method.request.path.proxy"]
  cache_namespace         = aws_api_gateway_resource.proxy_gateway_resource.id
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = "ANY"
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  resource_id             = aws_api_gateway_resource.proxy_gateway_resource.id
  rest_api_id             = aws_api_gateway_rest_api.rest_api.id
  timeout_milliseconds    = "29000"
  type                    = "AWS_PROXY"
  uri                     = "arn:aws:apigateway:us-east-2:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-2:854713338508:function:space-alert-resolver/invocations"
}
