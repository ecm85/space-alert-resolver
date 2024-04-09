resource "aws_api_gateway_integration" "gateway_integration" {
  cache_namespace         = aws_api_gateway_rest_api.rest_api.root_resource_id
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = aws_api_gateway_method.gateway_method.http_method
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  resource_id             = aws_api_gateway_rest_api.rest_api.root_resource_id
  rest_api_id             = aws_api_gateway_rest_api.rest_api.id
  timeout_milliseconds    = "29000"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.lambda.invoke_arn
}

resource "aws_api_gateway_integration" "proxy_gateway_integration" {
  cache_key_parameters    = ["method.request.path.proxy"]
  cache_namespace         = aws_api_gateway_resource.proxy_gateway_resource.id
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = aws_api_gateway_method.proxy_gateway_method.http_method
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  resource_id             = aws_api_gateway_resource.proxy_gateway_resource.id
  rest_api_id             = aws_api_gateway_rest_api.rest_api.id
  timeout_milliseconds    = "29000"
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.lambda.invoke_arn
}

resource "aws_api_gateway_integration_response" "gateway_integration_response" {
  http_method = aws_api_gateway_integration.gateway_integration.http_method
  resource_id = aws_api_gateway_rest_api.rest_api.root_resource_id
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  status_code = "200"
}

resource "aws_api_gateway_integration_response" "proxy_gateway_integration_response" {
  http_method = aws_api_gateway_integration.proxy_gateway_integration.http_method
  resource_id = aws_api_gateway_resource.proxy_gateway_resource.id
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  status_code = "200"
}

resource "aws_api_gateway_method" "gateway_method" {
  api_key_required = "false"
  authorization    = "NONE"
  http_method      = "ANY"
  resource_id      = aws_api_gateway_rest_api.rest_api.root_resource_id
  rest_api_id      = aws_api_gateway_rest_api.rest_api.id
}

resource "aws_api_gateway_method" "proxy_gateway_method" {
  api_key_required = "false"
  authorization    = "NONE"
  http_method      = "ANY"

  request_parameters = {
    "method.request.path.proxy" = "true"
  }

  resource_id = aws_api_gateway_resource.proxy_gateway_resource.id
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
}

resource "aws_api_gateway_method_response" "gateway_method_response" {
  http_method = aws_api_gateway_method.gateway_method.http_method
  resource_id = aws_api_gateway_rest_api.rest_api.root_resource_id
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  status_code = "200"
}

resource "aws_api_gateway_method_response" "proxy_gateway_method_response" {
  http_method = aws_api_gateway_method.proxy_gateway_method.http_method
  resource_id = aws_api_gateway_resource.proxy_gateway_resource.id
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  status_code = "200"
}

resource "aws_api_gateway_model" "empty_gateway_model" {
  content_type = "application/json"
  description  = "This is a default empty schema model"
  name         = "spacealertemptymodel"
  rest_api_id  = aws_api_gateway_rest_api.rest_api.id
  schema       = "{\n  \"$schema\": \"http://json-schema.org/draft-04/schema#\",\n  \"title\" : \"Empty Schema\",\n  \"type\" : \"object\"\n}"
}

resource "aws_api_gateway_model" "error_gateway_model" {
  content_type = "application/json"
  description  = "This is a default error schema model"
  name         = "spacealerterrormodel"
  rest_api_id  = aws_api_gateway_rest_api.rest_api.id
  schema       = "{\n  \"$schema\" : \"http://json-schema.org/draft-04/schema#\",\n  \"title\" : \"Error Schema\",\n  \"type\" : \"object\",\n  \"properties\" : {\n    \"message\" : { \"type\" : \"string\" }\n  }\n}"
}

resource "aws_api_gateway_resource" "proxy_gateway_resource" {
  parent_id   = aws_api_gateway_rest_api.rest_api.root_resource_id
  path_part   = "{proxy+}"
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
}

resource "aws_api_gateway_rest_api" "rest_api" {
  api_key_source               = "HEADER"
  binary_media_types           = ["*/*"]
  disable_execute_api_endpoint = "false"

  endpoint_configuration {
    types = ["REGIONAL"]
  }

  minimum_compression_size = "-1"
  name                     = "space-alert-resolver"
}

resource "aws_api_gateway_stage" "api_gateway_stage" {
  cache_cluster_enabled = "false"
  cache_cluster_size    = "0.5"
  deployment_id         =  aws_api_gateway_deployment.deployment.id
  rest_api_id           = aws_api_gateway_rest_api.rest_api.id
  stage_name            = "Live"
  xray_tracing_enabled  = "false"
}

resource "aws_api_gateway_deployment" "deployment" {
  rest_api_id = aws_api_gateway_rest_api.rest_api.id

  triggers = {
    redeployment = sha1(jsonencode(aws_api_gateway_rest_api.rest_api.body))
  }

  lifecycle {
    create_before_destroy = true
  }

  depends_on = [
      aws_api_gateway_method.gateway_method,
      aws_api_gateway_method.proxy_gateway_method,
      aws_api_gateway_integration.gateway_integration,
      aws_api_gateway_integration.proxy_gateway_integration,
    ]
}
