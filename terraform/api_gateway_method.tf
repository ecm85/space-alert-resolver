resource "aws_api_gateway_method" "gateway_method" {
  api_key_required = "false"
  authorization    = "NONE"
  http_method      = "ANY"
  resource_id      = aws_api_gateway_resource.gateway_resource.id
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
