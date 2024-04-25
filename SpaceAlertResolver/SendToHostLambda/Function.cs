using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using LambdaShared;

namespace SendToHostLambda
{
	internal class Function
	{
		private class SendToHostRequest
		{
			public string Code { get; set; }
			public string Text { get; set; }
		}

		public async Task<APIGatewayProxyResponse> FunctionHandler(
			APIGatewayProxyRequest request,
			ILambdaContext context
		)
		{
			var sendToHostRequest = JsonSerializer.Deserialize<SendToHostRequest>(request.Body);
			var requestContext = request.RequestContext;
			var connectionId = requestContext.ConnectionId;
			var gameService = new GameService();
			var webSocketService = new WebSocketService();
			var existingGame = await gameService.GetGame(sendToHostRequest.Code);
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
					await webSocketService.SendMessage(
						requestContext,
						"ClientMessageReceived",
						connectionId,
						new { sendToHostRequest.Text }
					);
				}
			}
			return new APIGatewayProxyResponse { StatusCode = 200 };
		}
	}
}
