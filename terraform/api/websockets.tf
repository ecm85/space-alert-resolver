resource "aws_apigatewayv2_api" "sockets_gateway" {
  name = "space-alert-sockets-gateway"
  protocol_type = "WEBSOCKET"
  route_selection_expression = "request.body.action"
}

resource "aws_apigatewayv2_deployment" "deployment" {
  api_id = aws_apigatewayv2_api.sockets_gateway.id

  lifecycle {
    create_before_destroy = true
  }

  triggers = {
    redeployment = sha1(join(",", tolist([
      jsonencode(aws_apigatewayv2_integration.create_game),
      jsonencode(aws_apigatewayv2_route.create_game),
    ])))
  }

  depends_on = [
    aws_apigatewayv2_route.create_game
  ]
}

resource "aws_apigatewayv2_stage" "Stage" {
  api_id        = aws_apigatewayv2_api.sockets_gateway.id
  name          = "Live"
  deployment_id = aws_apigatewayv2_deployment.deployment.id
  default_route_settings {
    data_trace_enabled = true
  }
}

resource "aws_apigatewayv2_route" "create_game" {
  api_id    = aws_apigatewayv2_api.sockets_gateway.id
  route_key = "CreateGame"
  target    = "integrations/${aws_apigatewayv2_integration.create_game.id}"
}

resource "aws_apigatewayv2_integration" "create_game" {
  api_id           = aws_apigatewayv2_api.sockets_gateway.id
  integration_type = "HTTP"
  integration_method = "POST"
  integration_uri = "https://space-alert-api.stormtide.net/Hub/CreateGame"
}
