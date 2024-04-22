resource "aws_dynamodb_table" "game_table" {
  name           = "Games"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "GameId"

  attribute {
    name = "GameId"
    type = "S"
  }
}
