using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using BLL.Threats;
using BLL.Tracks;

namespace ConsoleResolver
{
	public class Program
	{
		static void Main(string[] args)
		{
			var threats = new ExternalThreat[]
			{
				new Destroyer(3, ZoneType.Blue),
				new Fighter(4, ZoneType.White),
				new Fighter(2, ZoneType.Red)
			};
			var tracks = new Track []
			{
				new Track1(ZoneType.Blue),
				new Track3(ZoneType.Red),
				new Track7(ZoneType.White)
			};
			var players = new []
			{
				new Player
				{
					Actions = new PlayerAction[]{PlayerAction.None, PlayerAction.ChangeDeck, PlayerAction.B, PlayerAction.ChangeDeck, PlayerAction.A, PlayerAction.A, PlayerAction.A, PlayerAction.A, PlayerAction.A, PlayerAction.A,}
				}
			};
			const int numberOfTurns = 10;
			var game = new Game(threats, tracks, players, numberOfTurns);
			for(var i = 0; i < numberOfTurns; i++)
				game.PerformTurn();
			
		}
	}
}
