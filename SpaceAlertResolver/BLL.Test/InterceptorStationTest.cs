using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;
using NUnit.Framework;

namespace BLL.Test
{
	[TestFixture]
	public static class InterceptorStationTest
	{
		[Test]
		public static void TestInvalidActionInSpace()
		{
			var externalTracks = new Dictionary<ZoneLocation, TrackConfiguration>
			{
				{ZoneLocation.Red, TrackConfiguration.Track1 },
				{ ZoneLocation.White, TrackConfiguration.Track2},
				{ZoneLocation.Blue, TrackConfiguration.Track3}
			};
			var actionTypes = new[]
			{
				PlayerActionType.MoveRed,
				PlayerActionType.ChangeDeck,
				PlayerActionType.Charlie,
				PlayerActionType.ChangeDeck,
				PlayerActionType.Charlie,
				PlayerActionType.Charlie,
				PlayerActionType.MoveBlue,
				PlayerActionType.ChangeDeck
			};
			var expectedStations = new[]
			{
				StationLocation.UpperRed,
				StationLocation.LowerRed,
				StationLocation.LowerRed,
				StationLocation.UpperRed,
				StationLocation.Interceptor1,
				StationLocation.Interceptor2,
				StationLocation.Interceptor1,
				StationLocation.UpperRed,
				StationLocation.UpperWhite,
				StationLocation.LowerWhite,
				StationLocation.LowerWhite,
				StationLocation.LowerWhite
			};
			var actions = actionTypes
				.Select(action => new PlayerAction(action, null, null))
				.ToList();
			var computerActionTypes = new PlayerActionType?[]
			{
				PlayerActionType.Charlie, null, null,
				PlayerActionType.Charlie, null, null, null,
				PlayerActionType.Charlie, null, null, null, null
			};
			var computerActions = computerActionTypes
				.Select(action => new PlayerAction(action, null, null))
				.ToList();
			var player = new Player(actions, 0, PlayerColor.Green);
			var computerPlayer = new Player(computerActions, 1, PlayerColor.Purple);
			var players = new[] {player, computerPlayer};
			var game = new Game(players, new List<InternalThreat>(), new List<ExternalThreat>(), new List<Threat>(), externalTracks, TrackConfiguration.Track4, null);
			game.StartGame();
			for (var i = 0; i < game.NumberOfTurns; i++)
			{
				game.PerformTurn();
				Assert.AreEqual(expectedStations[i], player.CurrentStation.StationLocation);
			}
		}
	}
}
