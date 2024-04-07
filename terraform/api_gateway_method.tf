resource "aws_api_gateway_method" "tfer--ximufoi9a2-002F-g44r1ze2hl-002F-ANY" {
  api_key_required = "false"
  authorization    = "NONE"
  http_method      = "ANY"
  resource_id      = "g44r1ze2hl"
  rest_api_id      = "ximufoi9a2"
}

resource "aws_api_gateway_method" "tfer--ximufoi9a2-002F-g99mrh-002F-ANY" {
  api_key_required = "false"
  authorization    = "NONE"
  http_method      = "ANY"

  request_parameters = {
    "method.request.path.proxy" = "true"
  }

  resource_id = "g99mrh"
  rest_api_id = "ximufoi9a2"
}
