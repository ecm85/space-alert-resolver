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
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:-^b^aaaaaa",
				"player-index:1",
				"actions:<^c^-cxcxa",
				"player-index:2",
				"actions:-c--c^c",
				"player-index:3",
				"actions:^-----c",
				"player-index:4",
				"actions:-^----c",
				"player-index:5",
				"actions:3425",
				"-external-tracks",
				"blue:1",
				"red:2",
				"white:3",
				"-internal-track",
				"4",
				"-external-threats",
				"id:E1-02",
				"time:4",
				"location:blue",
				"id:E1-07",
				"time:5",
				"location:red",
				"id:E1-07",
				"time:6",
				"location:white",
				"-internal-threats",
				"id:I1-01",
				"time:4",
				"id:SI1-04",
				"time:2",
				"id:SI2-05",
				"time:6"
			};

			var game = Program.ParseArgsAndRunGame(args);

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
	}
}
