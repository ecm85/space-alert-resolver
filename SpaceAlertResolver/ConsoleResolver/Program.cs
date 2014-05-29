using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Tracks;

namespace ConsoleResolver
{
	public class Program
	{
		static void Main()
		{
			
			var players = new[]
			{
				new Player
				{
					Actions =
						new[]
						{
							PlayerAction.None, PlayerAction.ChangeDeck, PlayerAction.B, PlayerAction.ChangeDeck, PlayerAction.A,
							PlayerAction.A, PlayerAction.A, PlayerAction.A, PlayerAction.A, PlayerAction.A
						}
						//Enumerable.Repeat(PlayerAction.None, 10).ToList()
				}
			};
			var sittingDuck = new SittingDuck(players);
			var tracks = new Track[]
			{
				new Track1(sittingDuck.BlueZone),
				new Track2(sittingDuck.RedZone),
				new Track3(sittingDuck.WhiteZone)
			};
			var threats = new ExternalThreat[]
			{
				new Destroyer(7, sittingDuck.BlueZone),
				new Fighter(4, sittingDuck.RedZone),
				new Fighter(5, sittingDuck.WhiteZone)
			};

			const int numberOfTurns = 10;
			var game = new Game(sittingDuck, threats, tracks, players, numberOfTurns);
			for(var i = 0; i < numberOfTurns; i++)
				game.PerformTurn();
			Console.WriteLine("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
				sittingDuck.BlueZone.TotalDamage,
				sittingDuck.RedZone.TotalDamage,
				sittingDuck.WhiteZone.TotalDamage);
		}
	}
}
