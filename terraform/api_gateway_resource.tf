resource "aws_api_gateway_resource" "tfer--g44r1ze2hl" {
  parent_id   = ""
  path_part   = ""
  rest_api_id = "ximufoi9a2"
}

resource "aws_api_gateway_resource" "tfer--g99mrh" {
  parent_id   = "g44r1ze2hl"
  path_part   = "{proxy+}"
  rest_api_id = "ximufoi9a2"
}
