using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace LambdaShared
{
	public class GameService
	{
		public const string CreatedDateField = "CreatedDate";
		public const string GameIdField = "GameId";
		public const string ConnectionIdField = "ConnectionId";
		public const string GamesTableName = "Games";

		public async Task CreateGame(string gameId, string connectionId, string createdDate)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			await dynamoClient.PutItemAsync(
				new PutItemRequest(
					GamesTableName,
					new Dictionary<string, AttributeValue>()
					{
						[GameIdField] = new(gameId),
						[ConnectionIdField] = new(connectionId),
						[CreatedDateField] = new(createdDate)
					}
				)
			);
		}

		public async Task DeleteGame(string code)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			var gameIdLookup = CreateGameIdLookup(code);
			await dynamoClient.DeleteItemAsync(new DeleteItemRequest(GamesTableName, gameIdLookup));
		}

		public async Task<GetItemResponse> GetGame(string gameId)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			var gameIdLookup = CreateGameIdLookup(gameId);
			var existingGame = await dynamoClient.GetItemAsync(
				new GetItemRequest(GamesTableName, gameIdLookup)
			);
			return existingGame;
		}

		public Dictionary<string, AttributeValue> CreateGameIdLookup(string gameId)
		{
			var gameIdLookup = new Dictionary<string, AttributeValue>
			{
				[GameIdField] = new(gameId)
			};
			return gameIdLookup;
		}
	}
}
