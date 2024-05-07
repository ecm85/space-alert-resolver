using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using LambdaShared;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SendToHostLambda
{
	internal class Function
	{
		private class SendToHostRequest
		{
			public string Code { get; set; }
			public SendToHostRequestData Data { get; set; }
		}

		private class SendToHostRequestData
		{
			public string[][] BarcodeData { get; set; }
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
						$"Data from client 1: {JsonSerializer.Serialize(sendToHostRequest.Data)}"
					);
					context.Logger.Log(
						$"Data from client 2: {JsonSerializer.Serialize(sendToHostRequest.Data.BarcodeData[0])}"
					);
					var hostConnectionId = existingGame.Item[GameService.ConnectionIdField].S;
					await webSocketService.SendMessage(
						requestContext,
						"ClientMessageReceived",
						hostConnectionId,
						new { barcodeData = sendToHostRequest.Data.BarcodeData, connectionId }
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
