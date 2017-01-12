using System;

namespace BLL
{
	public enum GameStatus
	{
		InProgress,
		Lost,
		Won
	}

	public static class GameStatusExtensions
	{
		public static string GetDisplayName(this GameStatus status)
		{
			switch (status)
			{
				case GameStatus.InProgress:
					return "In Progress";
				case GameStatus.Lost:
					return "Lost";
				case GameStatus.Won:
					return "Won";
				default:
					throw new InvalidOperationException("Invalid game status!");
			}
		}
	}
}
