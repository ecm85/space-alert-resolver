module "api" {
  source = "./api"
}

module "pl" {
  source = "./pl"
}

module "ui" {
  source = "./ui"
}

module "socket_lambdas" {
  source = "./socketLambdas"
}
