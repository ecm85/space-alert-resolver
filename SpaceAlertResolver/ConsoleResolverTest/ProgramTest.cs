using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;
using BLL.Tracks;
using ConsoleResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleResolverTest
{
	[TestClass]
	public class ProgramTest
	{
		[TestMethod]
		public void TestBigCase()
		{
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:bba",
				"-external-threats",
				"id:E1-07",
				"time:3",
				"location:blue",
				"id:E1-08",
				"location:red",
				"time:8",
				"id:SE3-105",
				"time:9",
				"location:white",
				"extra-external-threat-id:E3-107",
				"extra-internal-threat-id:I1-06",
				"-internal-threats",
				"id:I1-05",
				"time:2",
				"id:SI3-102",
				"time:4",
				"extra-external-threat-id:E2-02",
				"-external-tracks",
				"red:2",
				"blue:3",
				"white:4",
				"-internal-track",
				"5"
			};
			Program.Main(args);
		}

		[TestMethod]
		public void JustAFighterNoActions()
		{
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:-",
				"-external-threats",
				"id:E1-07",
				"time:5",
				"location:red",
				"-internal-threats",
				"-internal-track",
				"4",
				"-external-tracks",
				"red:5",
				"blue:1",
				"white:3"
			};

			var game = Program.ParseArgsAndRunGame(args);

			Assert.AreEqual(0, game.SittingDuck.BlueZone.TotalDamage);
			Assert.AreEqual(5, game.SittingDuck.RedZone.TotalDamage);
			Assert.AreEqual(0, game.SittingDuck.WhiteZone.TotalDamage);
			Assert.AreEqual(0, game.ThreatController.DefeatedThreats.Count());
			Assert.AreEqual(1, game.ThreatController.SurvivedThreats.Count());
			Assert.AreEqual(2, game.TotalPoints);
			Assert.AreEqual(5, game.SittingDuck.Zones.ElementAt(0).AllDamageTokensTaken.Count());
			Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(1).AllDamageTokensTaken.Count());
			Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(2).AllDamageTokensTaken.Count());

			foreach (var zone in game.SittingDuck.Zones)
			{
				foreach (var token in zone.AllDamageTokensTaken)
					Console.WriteLine("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation, zone.CurrentDamageTokens.Contains(token));
			}
		}

		[TestMethod]
		public void SixBasicThreats()
		{
			var players = GetPlayers();

			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
			{
				{ZoneLocation.Blue, TrackConfiguration.Track1},
				{ZoneLocation.Red, TrackConfiguration.Track2},
				{ZoneLocation.White, TrackConfiguration.Track3},
			};
			var internalTrack = TrackConfiguration.Track4;

			var destroyer = new Destroyer { TimeAppears = 4, CurrentZone = ZoneLocation.Blue };
			var fighter1 = new Fighter { TimeAppears = 5, CurrentZone = ZoneLocation.Red };
			var fighter2 = new Fighter { TimeAppears = 6, CurrentZone = ZoneLocation.White };
			var externalThreats = new ExternalThreat[] { destroyer, fighter1, fighter2 };

			var skirmishers = new SkirmishersA { TimeAppears = 4 };
			var fissure = new Fissure { TimeAppears = 2 };
			var nuclearDevice = new NuclearDevice { TimeAppears = 6 };
			var internalThreats = new InternalThreat[] { skirmishers, fissure, nuclearDevice };

			var bonusThreats = new Threat[0];

			var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack);

			for (var currentTurn = 0; currentTurn < game.NumberOfTurns; currentTurn++)
				game.PerformTurn();

			Assert.AreEqual(4, game.SittingDuck.BlueZone.TotalDamage);
			Assert.AreEqual(3, game.SittingDuck.RedZone.TotalDamage);
			Assert.AreEqual(3, game.SittingDuck.WhiteZone.TotalDamage);
			Assert.AreEqual(3, game.ThreatController.DefeatedThreats.Count());
			Assert.AreEqual(3, game.ThreatController.SurvivedThreats.Count());
			Assert.AreEqual(30, game.TotalPoints);
			Assert.AreEqual(3, game.SittingDuck.Zones.ElementAt(0).AllDamageTokensTaken.Count());
			Assert.AreEqual(3, game.SittingDuck.Zones.ElementAt(1).AllDamageTokensTaken.Count());
			Assert.AreEqual(4, game.SittingDuck.Zones.ElementAt(2).AllDamageTokensTaken.Count());
		}

		private static IList<Player> GetPlayers()
		{
			var players = Enumerable.Range(0, 6).Select(index => new Player()).ToList();
			players[0].Actions = PlayerActionFactory.CreateSingleActionList(players[0], new PlayerActionType?[]
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
			});
			players[0].Index = 0;
			players[1].Actions = PlayerActionFactory.CreateSingleActionList(players[1], new PlayerActionType?[]
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
			});
			players[1].Index = 1;
			players[2].Actions = PlayerActionFactory.CreateSingleActionList(players[2], new PlayerActionType?[]
			{
				null,
				PlayerActionType.C,
				null,
				null,
				PlayerActionType.C,
				PlayerActionType.ChangeDeck,
				PlayerActionType.C
			});
			players[2].Index = 2;
			players[3].Actions = PlayerActionFactory.CreateSingleActionList(players[3], new PlayerActionType?[]
			{
				PlayerActionType.ChangeDeck,
				null,
				null,
				null,
				null,
				null,
				PlayerActionType.C
			});
			players[3].Index = 3;
			players[4].Actions = PlayerActionFactory.CreateSingleActionList(players[4], new PlayerActionType?[]
			{
				null,
				PlayerActionType.ChangeDeck,
				null,
				null,
				null,
				null,
				PlayerActionType.C
			});
			players[4].Index = 4;
			players[5].Actions = PlayerActionFactory.CreateSingleActionList(players[5], new PlayerActionType?[]
			{
				PlayerActionType.TeleportBlueLower,
				PlayerActionType.TeleportRedUpper,
				PlayerActionType.TeleportWhiteLower,
				PlayerActionType.TeleportWhiteUpper
			});
			players[5].Index = 5;
			return players;
		}
	}
}
