resource "aws_api_gateway_resource" "gateway_resource" {
  parent_id   = ""
  path_part   = ""
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
}

resource "aws_api_gateway_resource" "proxy_gateway_resource" {
  parent_id   = aws_api_gateway_resource.gateway_resource.id
  path_part   = "{proxy+}"
  rest_api_id = aws_api_gateway_rest_api.rest_api.id
}
