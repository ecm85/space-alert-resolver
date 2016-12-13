using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.Internal;
using BLL.Tracks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Threats;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;

namespace BLL.Test
{
	[TestClass]
	public class GameIntegrationTest
	{
		[TestMethod]
		public void JustAFighterNoActions()
		{
			var players = Enumerable.Range(0, 1).Select(index => new Player { Actions = new List<PlayerAction>() }).ToList();

			var externalTracksByZone = new Dictionary<ZoneLocation, TrackConfiguration>
			{
				{ZoneLocation.Blue, TrackConfiguration.Track1},
				{ZoneLocation.Red, TrackConfiguration.Track5},
				{ZoneLocation.White, TrackConfiguration.Track3}
			};
			var internalTrack = TrackConfiguration.Track4;

			var fighter = new Fighter { TimeAppears = 5, CurrentZone = ZoneLocation.Red };
			var externalThreats = new ExternalThreat[] { fighter };

			var internalThreats = new InternalThreat[0];
			var bonusThreats = new Threat[0];

			var game = new Game(players, internalThreats, externalThreats, bonusThreats, externalTracksByZone, internalTrack);

			for (var currentTurn = 0; currentTurn < game.NumberOfTurns; currentTurn++)
				game.PerformTurn();

			Assert.IsFalse(game.HasLost);
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

			Assert.IsFalse(game.HasLost);
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
