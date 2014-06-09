using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;
using BLL.Tracks;

namespace ConsoleResolver
{
	public class Program
	{
		static void Main()
		{

			var players = GetPlayers();
			var sittingDuck = new SittingDuck();
			sittingDuck.SetPlayers(players);
			var externalTracks = new[]
			{
				new ExternalTrack(TrackConfiguration.Track1, sittingDuck.BlueZone),
				new ExternalTrack(TrackConfiguration.Track2, sittingDuck.RedZone),
				new ExternalTrack(TrackConfiguration.Track3, sittingDuck.WhiteZone)
			};
			var internalTrack = new InternalTrack(TrackConfiguration.Track4);

			var destroyer = new Destroyer();
			var fighter1 = new Fighter();
			var fighter2 = new Fighter();
			var externalThreats = new ExternalThreat[] { destroyer, fighter1, fighter2 };
			var skirmishers = new SkirmishersA();
			var fissure = new Fissure();
			var nuclearDevice = new NuclearDevice();
			var internalThreats = new InternalThreat[] { skirmishers, fissure, nuclearDevice };
			var externalTracksByZone = externalTracks.ToDictionary(track => track.Zone.ZoneLocation);
			var threatController = new ThreatController(externalTracksByZone, internalTrack, externalThreats, internalThreats);
			destroyer.Initialize(sittingDuck, threatController, 3, ZoneLocation.Blue);
			fighter1.Initialize(sittingDuck, threatController, 4, ZoneLocation.Red);
			fighter2.Initialize(sittingDuck, threatController, 5, ZoneLocation.White);
			skirmishers.Initialize(sittingDuck, threatController, 3);
			fissure.Initialize(sittingDuck, threatController, 2);
			nuclearDevice.Initialize(sittingDuck, threatController, 5);
			var game = new Game(sittingDuck, players, threatController);
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
				game.ThreatController.ExternalThreats.Count(threat => threat.IsDefeated) + game.ThreatController.InternalThreats.Count(threat => threat.IsDefeated),
				game.ThreatController.ExternalThreats.Count(threat => threat.IsSurvived) + game.ThreatController.InternalThreats.Count(threat => threat.IsSurvived));
			Console.WriteLine("Total points: {0}", game.TotalPoints);
			foreach (var zone in sittingDuck.Zones)
			{
				foreach (var token in zone.AllDamageTokensTaken)
					Console.WriteLine("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation, zone.CurrentDamageTokens.Contains(token));
			}
		}

		private static Player[] GetPlayers()
		{
			var players = new[]
			{
				new Player
				{
					Actions =
						new List<PlayerAction>
						{
							PlayerAction.None,
							PlayerAction.ChangeDeck,
							PlayerAction.B,
							PlayerAction.ChangeDeck,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A,
							PlayerAction.A
						},
					Index = 1
				},
				new Player
				{
					Actions =
						new List<PlayerAction>
						{
							PlayerAction.MoveRed,
							PlayerAction.ChangeDeck,
							PlayerAction.C,
							PlayerAction.ChangeDeck,
							PlayerAction.None,
							PlayerAction.None,
							PlayerAction.C,
							PlayerAction.BattleBots
						},
					Index = 2
				},
				new Player
				{
					Actions =
						new List<PlayerAction> {PlayerAction.None, PlayerAction.C, PlayerAction.None, PlayerAction.None, PlayerAction.C, PlayerAction.ChangeDeck, PlayerAction.C},
						Index = 3
				},
				new Player
				{
					Actions = new List<PlayerAction>{PlayerAction.ChangeDeck, PlayerAction.None, PlayerAction.None, PlayerAction.None, PlayerAction.None, PlayerAction.None, PlayerAction.C},
					Index = 4
				},
				new Player
				{
					Actions = new List<PlayerAction>{PlayerAction.None, PlayerAction.ChangeDeck, PlayerAction.None, PlayerAction.None, PlayerAction.None, PlayerAction.None, PlayerAction.C},
					Index = 5
				}
			};
			return players;
		}
	}
}
