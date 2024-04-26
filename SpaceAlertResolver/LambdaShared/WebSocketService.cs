using System.Text;
using System.Text.Json;
using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LambdaShared
{
	public class WebSocketService
	{
		public async Task SendMessage(
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

		private class Request<T>
		{
			public T Data { get; set; }
			public string Action { get; set; }
		}

		public T DeserializeRequest<T>(string body)
		{
			return JsonConvert.DeserializeObject<Request<T>>(body).Data;
		}
	}
}
