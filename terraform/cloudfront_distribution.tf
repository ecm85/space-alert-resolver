data aws_acm_certificate cert {
  domain = "*.stormtide.net"
}

resource "aws_cloudfront_distribution" "cloudfront_distribution" {
  aliases = ["space-alert-resolver.stormtide.net"]

  default_cache_behavior {
    allowed_methods          = ["GET", "DELETE", "PATCH", "HEAD", "PUT", "OPTIONS", "POST"]
    cache_policy_id          = "4135ea2d-6df8-44a3-9df3-4b5a84be39ad"
    cached_methods           = ["HEAD", "GET"]
    compress                 = "false"
    default_ttl              = "0"
    max_ttl                  = "0"
    min_ttl                  = "0"
    origin_request_policy_id = "c3c6f699-3e2f-403e-a4d7-945521a3820c"
    smooth_streaming         = "false"
    target_origin_id         = "Space Alert Resolver Api Gateway"
    viewer_protocol_policy   = "redirect-to-https"
  }

  enabled         = "true"
  http_version    = "http2"
  is_ipv6_enabled = "true"

  origin {
    connection_attempts = "3"
    connection_timeout  = "10"

    custom_origin_config {
      http_port                = "80"
      https_port               = "443"
      origin_keepalive_timeout = "5"
      origin_protocol_policy   = "https-only"
      origin_read_timeout      = "30"
      origin_ssl_protocols     = ["TLSv1.2"]
    }

    domain_name = "ximufoi9a2.execute-api.us-east-2.amazonaws.com"
    origin_id   = "Space Alert Resolver Api Gateway"
    origin_path = "/Live"
  }

  price_class = "PriceClass_100"

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  retain_on_delete = "false"

  viewer_certificate {
    acm_certificate_arn            = data.aws_acm_certificate.cert.arn
    cloudfront_default_certificate = "false"
    minimum_protocol_version       = "TLSv1.2_2019"
    ssl_support_method             = "sni-only"
  }
}
