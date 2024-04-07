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
