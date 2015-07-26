using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.External.Minor.White;
using BLL.Threats.Internal;
using BLL.Threats.Internal.Minor.White;
using BLL.Threats.Internal.Serious.White;
using BLL.Threats.Internal.Serious.Yellow;
using BLL.Tracks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				{ZoneLocation.White, TrackConfiguration.Track3},
			};
			var internalTrack = TrackConfiguration.Track4;

			var fighter = new Fighter { TimeAppears = 5, CurrentZone = ZoneLocation.Red };
			var externalThreats = new ExternalThreat[] { fighter };

			var internalThreats = new InternalThreat[] { };

			var game = new Game(players, internalThreats, externalThreats, externalTracksByZone, internalTrack);

			var currentTurn = 0;
			for (currentTurn = 0; currentTurn < game.NumberOfTurns; currentTurn++)
				game.PerformTurn();
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
	}
}
