
resource "aws_api_gateway_integration" "tfer--ximufoi9a2-002F-g44r1ze2hl-002F-ANY" {
  cache_namespace         = "g44r1ze2hl"
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = "ANY"
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  resource_id             = "g44r1ze2hl"
  rest_api_id             = "ximufoi9a2"
  timeout_milliseconds    = "29000"
  type                    = "AWS_PROXY"
  uri                     = "arn:aws:apigateway:us-east-2:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-2:854713338508:function:space-alert-resolver/invocations"
}

resource "aws_api_gateway_integration" "tfer--ximufoi9a2-002F-g99mrh-002F-ANY" {
  cache_key_parameters    = ["method.request.path.proxy"]
  cache_namespace         = "g99mrh"
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = "ANY"
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  resource_id             = "g99mrh"
  rest_api_id             = "ximufoi9a2"
  timeout_milliseconds    = "29000"
  type                    = "AWS_PROXY"
  uri                     = "arn:aws:apigateway:us-east-2:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-2:854713338508:function:space-alert-resolver/invocations"
}
