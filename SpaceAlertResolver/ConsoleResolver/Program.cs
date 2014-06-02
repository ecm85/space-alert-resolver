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
					Actions = new List<PlayerAction>
					{
						PlayerAction.None, PlayerAction.ChangeDeck, PlayerAction.B, PlayerAction.ChangeDeck, PlayerAction.A,
						PlayerAction.A, PlayerAction.A, PlayerAction.A, PlayerAction.A, PlayerAction.A
					},
					Index = 1
				},
				new Player
				{
					Actions = new List<PlayerAction>
					{
						PlayerAction.MoveRed, PlayerAction.ChangeDeck, PlayerAction.C, PlayerAction.ChangeDeck, PlayerAction.None, PlayerAction.None, PlayerAction.C, PlayerAction.BattleBots
					},
					Index = 2
				},
				new Player
				{
					Actions = new List<PlayerAction>
					{
						PlayerAction.None, PlayerAction.C, PlayerAction.None, PlayerAction.None, PlayerAction.C
					}
				}
			};
			var sittingDuck = new SittingDuck(players);
			var externalTracks = new []
			{
				new ExternalTrack(TrackConfiguration.Track1, sittingDuck.BlueZone),
				new ExternalTrack(TrackConfiguration.Track2, sittingDuck.RedZone),
				new ExternalTrack(TrackConfiguration.Track3, sittingDuck.WhiteZone)
			};
			var externalThreats = new ExternalThreat[]
			{
				new Destroyer(3, sittingDuck.BlueZone, sittingDuck),
				new Fighter(4, sittingDuck.RedZone, sittingDuck),
				new Fighter(5, sittingDuck.WhiteZone, sittingDuck)
			};
			var internalTrack = new InternalTrack(TrackConfiguration.Track4);
			var internalThreats = new InternalThreat[]
			{
				new SkirmishersA(3, sittingDuck),
				new Fissure(2, sittingDuck)
				//new Alien(1, sittingDuck)
			};
			var game = new Game(sittingDuck, externalThreats, externalTracks, internalThreats, internalTrack, players);
			var currentTurn = 0;
			try
			{
				for (currentTurn = 0; currentTurn < Game.NumberOfTurns; currentTurn++)
					game.PerformTurn();
			}
			catch (LoseException loseException)
			{
				Console.WriteLine("Killed on turn {0} by: {1}", currentTurn, loseException.Threat);
			}
			Console.WriteLine("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
				sittingDuck.BlueZone.TotalDamage,
				sittingDuck.RedZone.TotalDamage,
				sittingDuck.WhiteZone.TotalDamage);
			Console.WriteLine("Threats killed: {0}. Threats survived: {1}",
				game.defeatedThreats.Count,
				game.survivedThreats.Count);
			Console.WriteLine("Total points: {0}", game.TotalPoints);
			foreach (var zone in sittingDuck.Zones)
			{
				foreach (var token in zone.AllDamageTokensTaken)
					Console.WriteLine("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation, zone.CurrentDamageTokens.Contains(token));
			}
		}
	}
}
