resource "aws_apigatewayv2_api" "sockets_gateway" {
  name = "space-alert-sockets-gateway"
  protocol_type = "WEBSOCKET"
  route_selection_expression = "request.body.action"
}

resource "aws_apigatewayv2_deployment" "deployment" {
  api_id = aws_apigatewayv2_api.sockets_gateway.id

  depends_on = [
    aws_apigatewayv2_route.start_game
  ]
}

resource "aws_apigatewayv2_stage" "Stage" {
  api_id        = aws_apigatewayv2_api.sockets_gateway.id
  name          = "Live"
  deployment_id = aws_apigatewayv2_deployment.deployment.id
}

resource "aws_apigatewayv2_route" "start_game" {
  api_id    = aws_apigatewayv2_api.sockets_gateway.id
  route_key = "StartGame"
  target    = "integrations/${aws_apigatewayv2_integration.start_game.id}"
}

resource "aws_apigatewayv2_integration" "start_game" {
  api_id           = aws_apigatewayv2_api.sockets_gateway.id
  integration_type = "HTTP"
  http_method      = "POST"
  integration_uri = "https://space-alert-api.stormtide.net/Hub/StartGame"
}
