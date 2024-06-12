using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using LambdaShared;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SendToHostLambda
{
	internal class Function
	{
		private class SendToHostRequest
		{
			public string Code { get; set; }
			public JObject Message { get; set; }
		}

		public async Task<APIGatewayProxyResponse> FunctionHandler(
			APIGatewayProxyRequest request,
			ILambdaContext context
		)
		{
			var gameService = new GameService();
			var webSocketService = new WebSocketService();
			var sendToHostRequest = webSocketService.DeserializeRequest<SendToHostRequest>(
				request.Body
			);
			var requestContext = request.RequestContext;
			var connectionId = requestContext.ConnectionId;

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
					context.Logger.Log(
						$"Data from client: {JsonSerializer.Serialize(request.Body)}"
					);
					var hostConnectionId = existingGame.Item[GameService.ConnectionIdField].S;
					await webSocketService.SendMessage(
						requestContext,
						"ClientMessageReceived",
						hostConnectionId,
						new { sendToHostRequest.Message, connectionId }
					);
					await webSocketService.SendMessage(
						requestContext,
						"YourMessageSent",
						connectionId
					);
				}
			}
			return new APIGatewayProxyResponse { StatusCode = 200 };
		}
	}
}
