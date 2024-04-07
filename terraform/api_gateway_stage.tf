resource "aws_api_gateway_stage" "api_gateway_stage" {
  cache_cluster_enabled = "false"
  cache_cluster_size    = "0.5"
  deployment_id         = "coz1x5"
  rest_api_id           = aws_api_gateway_rest_api.rest_api.id
  stage_name            = "Live"
  xray_tracing_enabled  = "false"
}
