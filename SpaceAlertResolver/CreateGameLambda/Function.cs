using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using LambdaShared;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateGameLambda
{
	public class Function
	{
		private static Random Random { get; } = new();

		public async Task<APIGatewayProxyResponse> FunctionHandler(
			APIGatewayProxyRequest request,
			ILambdaContext context
		)
		{
			var requestContext = request.RequestContext;
			var connectionId = requestContext.ConnectionId;
			var gameService = new GameService();
			var webSocketService = new WebSocketService();
			var code = (string)null;
			do
			{
				var attempt = RandomString(4);
				var existingGame = await gameService.GetGame(attempt);
				if (existingGame.Item.Keys.Count == 0)
				{
					code = attempt;
				}
				else
				{
					var dayCreated = DateTime.Parse(
						existingGame.Item[GameService.CreatedDateField].S
					);
					if (dayCreated < DateTime.Today.AddDays(-7))
					{
						await gameService.DeleteGame(attempt);
						code = attempt;
					}
				}
			} while (code == null);

			var gameId = code;
			var createdDate = DateTime.Today.ToShortDateString();
			await gameService.CreateGame(gameId, connectionId, createdDate);
			await webSocketService.SendMessage(
				requestContext,
				"GameCreated",
				connectionId,
				new { code }
			);

			return new APIGatewayProxyResponse { StatusCode = 200 };
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
