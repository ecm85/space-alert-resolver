using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using LambdaShared;

namespace JoinGameLambda
{
	internal class Function
	{
		private class JoinGameRequest
		{
			public string Code { get; set; }
			public string Name { get; set; }
		}

		public async Task<APIGatewayProxyResponse> FunctionHandler(
			APIGatewayProxyRequest request,
			ILambdaContext context
		)
		{
			var gameService = new GameService();
			var webSocketService = new WebSocketService();

			var joinGameRequest = JsonSerializer.Deserialize<JoinGameRequest>(request.Body);
			var requestContext = request.RequestContext;
			var connectionId = requestContext.ConnectionId;
			var existingGame = await gameService.GetGame(joinGameRequest.Code);
			if (existingGame.Item == null)
			{
				await webSocketService.SendMessage(requestContext, "InvalidGameCode", connectionId);
			}
			else
			{
				var dayCreated = DateTime.Parse(existingGame.Item[GameService.CreatedDateField].S);
				if (dayCreated < DateTime.Today.AddDays(-7))
				{
					await webSocketService.SendMessage(
						requestContext,
						"ExpiredGameCode",
						connectionId
					);
				}
				else
				{
					var hostConnectionId = existingGame.Item[GameService.ConnectionIdField].S;
					await webSocketService.SendMessage(requestContext, "YouJoined", connectionId);
					await webSocketService.SendMessage(
						requestContext,
						"ClientJoined",
						hostConnectionId,
						new { joinGameRequest.Name }
					);
				}
			}
			return new APIGatewayProxyResponse { StatusCode = 200 };
		}
	}
}
