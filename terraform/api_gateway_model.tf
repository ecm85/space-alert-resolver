resource "aws_api_gateway_model" "tfer--4dapjj" {
  content_type = "application/json"
  description  = "This is a default empty schema model"
  name         = "Empty"
  rest_api_id  = "ximufoi9a2"
  schema       = "{\n  \"$schema\": \"http://json-schema.org/draft-04/schema#\",\n  \"title\" : \"Empty Schema\",\n  \"type\" : \"object\"\n}"
}

resource "aws_api_gateway_model" "tfer--tk5cyd" {
  content_type = "application/json"
  description  = "This is a default error schema model"
  name         = "Error"
  rest_api_id  = "ximufoi9a2"
  schema       = "{\n  \"$schema\" : \"http://json-schema.org/draft-04/schema#\",\n  \"title\" : \"Error Schema\",\n  \"type\" : \"object\",\n  \"properties\" : {\n    \"message\" : { \"type\" : \"string\" }\n  }\n}"
}
