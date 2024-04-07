resource "aws_api_gateway_method_response" "gateway_method_response" {
  http_method = "ANY"
  resource_id = aws_api_gateway_resource.gateway_resource.id
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  status_code = "200"
}

resource "aws_api_gateway_method_response" "proxy_gateway_method_response" {
  http_method = "ANY"
  resource_id = aws_api_gateway_resource.proxy_gateway_resource.id
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
  status_code = "200"
}
