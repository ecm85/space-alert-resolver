using System;
using System.Linq;
using BLL;
using ConsoleResolver;
using NUnit.Framework;

namespace ConsoleResolverTest
{
	[TestFixture]
	public static class ProgramTest
	{
		[Test]
		public static void TestBigCase()
		{
			//TODO: This test isn't that useful. Make it so!
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:bba",
				"player-color: red",
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

		[Test]
		public static void JustAFighterNoActions()
		{
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:-",
				"player-color: red",
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

			var game = Program.ParseAndRunGame(args);

			Assert.AreEqual(GameStatus.Won, game.GameStatus);
			Assert.AreEqual(0, game.SittingDuck.BlueZone.TotalDamage);
			Assert.AreEqual(5, game.SittingDuck.RedZone.TotalDamage);
			Assert.AreEqual(0, game.SittingDuck.WhiteZone.TotalDamage);
			Assert.AreEqual(0, game.ThreatController.DefeatedThreats.Count());
			Assert.AreEqual(1, game.ThreatController.SurvivedThreats.Count());
			Assert.AreEqual(2, game.TotalPoints);
			Assert.AreEqual(5, game.SittingDuck.Zones.ElementAt(0).CurrentDamageTokens.Count());
			Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(1).CurrentDamageTokens.Count());
			Assert.AreEqual(0, game.SittingDuck.Zones.ElementAt(2).CurrentDamageTokens.Count());

			foreach (var zone in game.SittingDuck.Zones)
			{
				foreach (var token in zone.CurrentDamageTokens)
					Console.WriteLine("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation, zone.CurrentDamageTokens.Contains(token));
			}
		}

		[Test]
		public static void SixBasicThreats()
		{
			var args = new[]
			{
				"-players",
				"player-index:0",
				"actions:-^b^aaaaaa",
				"player-color: red",
				"player-index:1",
				"actions:<^c^-cxcxa",
				"player-color: blue",
				"player-index:2",
				"actions:-c--c^c",
				"player-color: green",
				"player-index:3",
				"actions:^-----c",
				"player-color: yellow",
				"player-index:4",
				"actions:-^----c",
				"player-color: purple",
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

			var game = Program.ParseAndRunGame(args);

			Assert.AreEqual(GameStatus.Won, game.GameStatus);
			Assert.AreEqual(4, game.SittingDuck.BlueZone.TotalDamage);
			Assert.AreEqual(3, game.SittingDuck.RedZone.TotalDamage);
			Assert.AreEqual(3, game.SittingDuck.WhiteZone.TotalDamage);
			Assert.AreEqual(3, game.ThreatController.DefeatedThreats.Count());
			Assert.AreEqual(3, game.ThreatController.SurvivedThreats.Count());
			Assert.AreEqual(30, game.TotalPoints);
			Assert.AreEqual(3, game.SittingDuck.Zones.ElementAt(0).CurrentDamageTokens.Count());
			Assert.AreEqual(3, game.SittingDuck.Zones.ElementAt(1).CurrentDamageTokens.Count());
			Assert.AreEqual(4, game.SittingDuck.Zones.ElementAt(2).CurrentDamageTokens.Count());
		}
	}
}
