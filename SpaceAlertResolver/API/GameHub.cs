using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.SignalR;

namespace API
{
	public class GameHub : Hub
	{
		private static Random Random { get; } = new();

		public async Task StartGame()
		{
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
					var dayCreated = DateTime.Parse(existingGame.Item["CreatedDate"].S);
					if (dayCreated < DateTime.Today.AddDays(-7))
					{
						await DeleteGame(attempt);
						await Groups.RemoveFromGroupAsync(
							existingGame.Item["ConnectionId"].S,
							attempt
						);
						code = attempt;
					}
				}
			} while (code == null);

			var gameId = code;
			var connectionId = Context.ConnectionId;
			var createdDate = DateTime.Today.ToShortDateString();
			await CreateGame(gameId, connectionId, createdDate);
			await Groups.AddToGroupAsync(Context.ConnectionId, code);
			await Clients.Caller.SendAsync("GameCreated", code);
		}

		private static async Task CreateGame(string gameId, string connectionId, string createdDate)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			await dynamoClient.PutItemAsync(
				new PutItemRequest(
					"Games",
					new Dictionary<string, AttributeValue>()
					{
						["GameId"] = new(gameId),
						["ConnectionId"] = new(connectionId),
						["CreatedDate"] = new(createdDate)
					}
				)
			);
		}

		private static async Task DeleteGame(string code)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			var gameIdLookup = CreateGameIdLookup(code);
			await dynamoClient.DeleteItemAsync(new DeleteItemRequest("Games", gameIdLookup));
		}

		private static async Task<GetItemResponse> GetGame(string gameId)
		{
			using var dynamoClient = new AmazonDynamoDBClient();
			var gameIdLookup = CreateGameIdLookup(gameId);
			var existingGame = await dynamoClient.GetItemAsync(
				new GetItemRequest("Games", gameIdLookup)
			);
			return existingGame;
		}

		private static Dictionary<string, AttributeValue> CreateGameIdLookup(string gameId)
		{
			var gameIdLookup = new Dictionary<string, AttributeValue> { ["GameId"] = new(gameId) };
			return gameIdLookup;
		}

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(
				Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray()
			);
		}

		public async Task JoinGame(string code, string name)
		{
			var existingGame = await GetGame(code);
			if (existingGame.Item == null)
			{
				throw new HubException("Invalid Game");
			}
			else
			{
				var dayCreated = DateTime.Parse(existingGame.Item["CreatedDate"].S);
				if (dayCreated < DateTime.Today.AddDays(-7))
				{
					// TODO: Delete game and remove from connection?
					throw new HubException("Expired Game");
				}
				else
				{
					await Clients.Caller.SendAsync("YouJoined");
					await Clients.Group(code).SendAsync("ClientJoined", name);
				}
			}
		}

		public async Task SendToServer(string text, string code)
		{
			var existingGame = await GetGame(code);
			if (existingGame.Item == null)
			{
				throw new HubException("Invalid Game");
			}
			else
			{
				var dayCreated = DateTime.Parse(existingGame.Item["CreatedDate"].S);
				if (dayCreated < DateTime.Today.AddDays(-7))
				{
					// TODO: Delete game and remove from connection?
					throw new HubException("Expired Game");
				}
				else
				{
					await Clients.Group(code).SendAsync("ClientMessageReceived", text);
				}
			}
		}
	}
}
