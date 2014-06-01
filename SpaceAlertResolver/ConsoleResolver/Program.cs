using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using BLL.Threats.External;
using BLL.Threats.Internal;
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
			var externalTracks = new []
			{
				new ExternalTrack(TrackConfiguration.Track1, sittingDuck.BlueZone),
				new ExternalTrack(TrackConfiguration.Track2, sittingDuck.RedZone),
				new ExternalTrack(TrackConfiguration.Track3, sittingDuck.WhiteZone),
			};
			var externalThreats = new ExternalThreat[]
			{
				new Destroyer(7, sittingDuck.BlueZone, sittingDuck),
				new Fighter(4, sittingDuck.RedZone, sittingDuck),
				new Fighter(5, sittingDuck.WhiteZone, sittingDuck)
			};
			var internalTrack = new InternalTrack(TrackConfiguration.Track4);
			var internalThreats = new InternalThreat[]
			{
				new SkirmishersA(3, sittingDuck)
			};
			var game = new Game(sittingDuck, externalThreats, externalTracks, internalThreats, internalTrack, players);
			for(var i = 0; i < Game.NumberOfTurns; i++)
				game.PerformTurn();
			Console.WriteLine("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
				sittingDuck.BlueZone.TotalDamage,
				sittingDuck.RedZone.TotalDamage,
				sittingDuck.WhiteZone.TotalDamage);
			Console.WriteLine("Threats killed: {0}. Threats survived: {1}",
				game.defeatedThreats.Count,
				game.survivedThreats.Count);
		}
	}
}
