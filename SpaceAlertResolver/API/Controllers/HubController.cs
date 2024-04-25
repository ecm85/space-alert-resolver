using System.Text;
using System.Text.Json;
using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HubController : Controller
	{
		private static Random Random { get; } = new();
		private const string CreatedDateField = "CreatedDate";
		private const string GameIdField = "GameId";
		private const string ConnectionIdField = "ConnectionId";
		private const string GamesTableName = "Games";

		private class JoinGameRequest
		{
			public string Code { get; set; }
			public string Name { get; set; }
		}

		private class SendToHostRequest
		{
			public string Code { get; set; }
			public string Text { get; set; }
		}

		[HttpPost]
		[Route("CreateGame")]
		public async Task<IActionResult> CreateGame([FromBody] APIGatewayProxyRequest request)
		{
			var requestContext = request.RequestContext;
			var connectionId = requestContext.ConnectionId;
			var code = (string)null;
			do
			{
				var attempt = RandomString(4);
				var existingGame = await GetGame(attempt);
				if (existingGame.Item.Keys.Count == 0)
				{
					code = attempt;
				}
				else
				{
					var dayCreated = DateTime.Parse(existingGame.Item[CreatedDateField].S);
					if (dayCreated < DateTime.Today.AddDays(-7))
					{
						await DeleteGame(attempt);
						code = attempt;
					}
				}
			} while (code == null);

			var gameId = code;
			var createdDate = DateTime.Today.ToShortDateString();
			await CreateGame(gameId, connectionId, createdDate);
			await SendMessage(requestContext, "GameCreated", connectionId, new { code });
			return Ok();
		}

		[HttpPost]
		[Route("JoinGame")]
		public async Task<IActionResult> JoinGame([FromBody] APIGatewayProxyRequest request)
		{
			var joinGameRequest = JsonSerializer.Deserialize<JoinGameRequest>(request.Body);
			var requestContext = request.RequestContext;
			var connectionId = requestContext.ConnectionId;
			var existingGame = await GetGame(joinGameRequest.Code);
			if (existingGame.Item == null)
			{
				await SendMessage(requestContext, "InvalidGameCode", connectionId);
			}
			else
			{
				var dayCreated = DateTime.Parse(existingGame.Item[CreatedDateField].S);
				if (dayCreated < DateTime.Today.AddDays(-7))
				{
					await SendMessage(requestContext, "ExpiredGameCode", connectionId);
				}
				else
				{
					var hostConnectionId = existingGame.Item[ConnectionIdField].S;
					await SendMessage(requestContext, "YouJoined", connectionId);
					await SendMessage(
						requestContext,
						"ClientJoined",
						hostConnectionId,
						new { joinGameRequest.Name }
					);
				}
			}

			return Ok();
		}

		[HttpPost]
		[Route("SendToHost")]
		public async Task<IActionResult> SendToHost([FromBody] APIGatewayProxyRequest request)
		{
			var sendToHostRequest = JsonSerializer.Deserialize<SendToHostRequest>(request.Body);
			var requestContext = request.RequestContext;
			var connectionId = requestContext.ConnectionId;
			var existingGame = await GetGame(sendToHostRequest.Code);
			if (existingGame.Item == null)
			{
				await SendMessage(requestContext, "InvalidGameCode", connectionId);
			}
			else
			{
				var dayCreated = DateTime.Parse(existingGame.Item[CreatedDateField].S);
				if (dayCreated < DateTime.Today.AddDays(-7))
				{
					await SendMessage(requestContext, "ExpiredGameCode", connectionId);
				}
				else
				{
					await SendMessage(
						requestContext,
						"ClientMessageReceived",
						connectionId,
						new { sendToHostRequest.Text }
					);
				}
			}

			return Ok();
		}

		private async Task SendMessage(
			APIGatewayProxyRequest.ProxyRequestContext requestContext,
			string @event,
			string connectionId,
			object data = null
		)
		{
			using var stream = new MemoryStream(
				Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { @event, data }))
			);
			var endpoint = $"https://{requestContext.DomainName}/{requestContext.Stage}";

			var apiClient = new AmazonApiGatewayManagementApiClient(
				new AmazonApiGatewayManagementApiConfig { ServiceURL = endpoint }
			);
			var postConnectionRequest = new PostToConnectionRequest
			{
				ConnectionId = connectionId,
				Data = stream
			};
			stream.Position = 0;
			await apiClient.PostToConnectionAsync(postConnectionRequest);
		}

		private static async Task CreateGame(string gameId, string connectionId, string createdDate)
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

		private static async Task DeleteGame(string code)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			var gameIdLookup = CreateGameIdLookup(code);
			await dynamoClient.DeleteItemAsync(new DeleteItemRequest(GamesTableName, gameIdLookup));
		}

		private static async Task<GetItemResponse> GetGame(string gameId)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			var gameIdLookup = CreateGameIdLookup(gameId);
			var existingGame = await dynamoClient.GetItemAsync(
				new GetItemRequest(GamesTableName, gameIdLookup)
			);
			return existingGame;
		}

		private static Dictionary<string, AttributeValue> CreateGameIdLookup(string gameId)
		{
			var gameIdLookup = new Dictionary<string, AttributeValue>
			{
				[GameIdField] = new(gameId)
			};
			return gameIdLookup;
		}

		private static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(
				Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray()
			);
		}
	}
}
