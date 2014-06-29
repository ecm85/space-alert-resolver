using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using BLL.ShipComponents;
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
			
			var externalTracksByZone = new Dictionary<ZoneLocation, Track>
			{
				{ZoneLocation.Blue, new Track(TrackConfiguration.Track1)},
				{ZoneLocation.Red, new Track(TrackConfiguration.Track2)},
				{ZoneLocation.White, new Track(TrackConfiguration.Track3)},
			};
			var internalTrack = new Track(TrackConfiguration.Track4);
			var destroyer = new Destroyer();
			var fighter1 = new Fighter();
			var fighter2 = new Fighter();
			var externalThreats = new ExternalThreat[] { destroyer, fighter1, fighter2 };
			var skirmishers = new SkirmishersA();
			var fissure = new Fissure();
			var nuclearDevice = new NuclearDevice();
			var internalThreats = new InternalThreat[] { skirmishers, fissure, nuclearDevice };
			var threatController = new ThreatController(externalTracksByZone, internalTrack, externalThreats, internalThreats);
			var game = new Game(players, threatController);
			var sittingDuck = game.SittingDuck;
			sittingDuck.SetPlayers(players);
			destroyer.Initialize(sittingDuck, threatController, 4, ZoneLocation.Blue);
			fighter1.Initialize(sittingDuck, threatController, 5, ZoneLocation.Red);
			fighter2.Initialize(sittingDuck, threatController, 6, ZoneLocation.White);
			skirmishers.Initialize(sittingDuck, threatController, 4);
			fissure.Initialize(sittingDuck, threatController, 2);
			nuclearDevice.Initialize(sittingDuck, threatController, 6);
			
			var currentTurn = 0;
			try
			{
				for (currentTurn = 0; currentTurn < game.NumberOfTurns; currentTurn++)
					game.PerformTurn();
			}
			catch (LoseException loseException)
			{
				Console.WriteLine("Killed on turn {0} by: {1}", currentTurn + 1, loseException.Threat);
			}
			Console.WriteLine("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
				sittingDuck.BlueZone.TotalDamage,
				sittingDuck.RedZone.TotalDamage,
				sittingDuck.WhiteZone.TotalDamage);
			Console.WriteLine("Threats killed: {0}. Threats survived: {1}", threatController.DefeatedThreats.Count(), threatController.SurvivedThreats.Count());
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
					Actions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
					{
						null,
						PlayerActionType.ChangeDeck,
						PlayerActionType.B,
						PlayerActionType.ChangeDeck,
						PlayerActionType.A,
						PlayerActionType.A,
						PlayerActionType.A,
						PlayerActionType.A,
						PlayerActionType.A,
						PlayerActionType.A
					}),
					Index = 0
				},
				new Player
				{
					Actions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
					{
						PlayerActionType.MoveRed,
						PlayerActionType.ChangeDeck,
						PlayerActionType.C,
						PlayerActionType.ChangeDeck,
						null,
						PlayerActionType.C,
						PlayerActionType.BattleBots,
						PlayerActionType.C,
						PlayerActionType.BattleBots,
						PlayerActionType.A
					}),
					Index = 1
				},
				new Player
				{
					Actions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
					{
						null,
						PlayerActionType.C,
						null,
						null,
						PlayerActionType.C,
						PlayerActionType.ChangeDeck,
						PlayerActionType.C
					}),
					Index = 2
				},
				new Player
				{
					Actions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
					{
						PlayerActionType.ChangeDeck,
						null,
						null,
						null,
						null,
						null,
						PlayerActionType.C
					}),
					Index = 3
				},
				new Player
				{
					Actions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
					{
						null,
						PlayerActionType.ChangeDeck,
						null,
						null,
						null,
						null,
						PlayerActionType.C
					}),
					Index = 4
				},
				new Player
				{
					Actions = PlayerActionFactory.CreateSingleActionList(new PlayerActionType?[]
					{
						PlayerActionType.TeleportBlueLower,
						PlayerActionType.TeleportRedUpper,
						PlayerActionType.TeleportWhiteLower,
						PlayerActionType.TeleportWhiteUpper
					}),
					Index = 5
				}
			};
			return players;
		}
	}
}
