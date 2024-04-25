terraform {
  required_version = "1.7.5"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "5.44.0"
      configuration_aliases = [ aws.east ]
    }
  }

  backend "s3" {
    bucket         = "ecm85-terraform"
    key            = "space-alert-resolver"
    region         = "us-east-2"
    dynamodb_table = "tfstate-lock"
  }
}

provider "aws" {
  region = "us-east-2"
}

provider "aws" {
  region = "us-east-1"
  alias  = "east"
}
