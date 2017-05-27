using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Common;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL
{
	public static class MovementController
	{
		public static void ChangeDeck(
			IDictionary<StationLocation, StandardStation> standardStationsByLocation,
			Player player,
			int currentTurn)
		{
			Check.ArgumentIsNotNull(standardStationsByLocation, "standardStationsByLocation");
			Check.ArgumentIsNotNull(player, "player");
			var oldStation = standardStationsByLocation[player.CurrentStation.StationLocation];
			var newStation = standardStationsByLocation[player.CurrentStation.StationLocation.OppositeStationLocation().GetValueOrDefault()];
			if (newStation != null)
			{
				var movedOut = oldStation.PerformMoveOutTowardsOppositeDeck(player, currentTurn, false);
				if (movedOut)
					newStation.MovePlayerIn(player, currentTurn);
			}
		}

		public static void MoveBlue(
			IDictionary<StationLocation, StandardStation> standardStationsByLocation,
			Player player,
			int currentTurn)
		{
			Check.ArgumentIsNotNull(standardStationsByLocation, "standardStationsByLocation");
			Check.ArgumentIsNotNull(player, "player");
			var oldStation = standardStationsByLocation[player.CurrentStation.StationLocation];
			var newStation = standardStationsByLocation[player.CurrentStation.StationLocation.BluewardStationLocation().GetValueOrDefault()];
			if (newStation != null)
			{
				var movedOut = oldStation.PerformMoveOutTowardsBlue(player, currentTurn);
				if (movedOut)
					newStation.MovePlayerIn(player, currentTurn);
			}
		}

		public static void MoveRed(
			IDictionary<StationLocation, StandardStation> standardStationsByLocation,
			Player player,
			int currentTurn)
		{
			Check.ArgumentIsNotNull(standardStationsByLocation, "standardStationsByLocation");
			Check.ArgumentIsNotNull(player, "player");
			var oldStation = standardStationsByLocation[player.CurrentStation.StationLocation];
			var newStation = standardStationsByLocation[player.CurrentStation.StationLocation.RedwardStationLocation().GetValueOrDefault()];
			if (newStation != null)
			{
				var movedOut = oldStation.PerformMoveOutTowardsRed(player, currentTurn);
				if (movedOut)
					newStation.MovePlayerIn(player, currentTurn);
			}
		}

		public static void MoveHeroically(
			IDictionary<StationLocation, StandardStation> standardStationsByLocation,
			Player performingPlayer,
			StationLocation newStationLocation,
			int currentTurn)
		{
			Check.ArgumentIsNotNull(standardStationsByLocation, "standardStationsByLocation");
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			var currentStationLocation = performingPlayer.CurrentStation.StationLocation;
			if (currentStationLocation == newStationLocation)
				return;
			var currentStation = standardStationsByLocation[currentStationLocation];
			var path = Path(currentStationLocation, newStationLocation)
				.Select(stationLocation => standardStationsByLocation[stationLocation])
				.Select((station, index) => new {Station = station, Index = index})
				.ToList();
			var firstDestination = path.First();
			var movedOut = MoveOut(performingPlayer, currentTurn, currentStation, firstDestination.Station.StationLocation);
			if (!movedOut)
				return;
			var finalDestination = path.First(
				pathLocation => pathLocation.Index >= path.Count - 1 ||
				!CanMoveOut(pathLocation.Station, path[pathLocation.Index + 1].Station.StationLocation));
			finalDestination.Station.MovePlayerIn(performingPlayer, currentTurn);
		}

		private static bool MoveOut(
			Player performingPlayer,
			int currentTurn,
			StandardStation fromStation,
			StationLocation toStationLocation)
		{
			if (toStationLocation == fromStation.StationLocation.OppositeStationLocation())
				return fromStation.PerformMoveOutTowardsOppositeDeck(performingPlayer, currentTurn, true);
			if (toStationLocation == fromStation.StationLocation.RedwardStationLocation())
				return fromStation.PerformMoveOutTowardsRed(performingPlayer, currentTurn);
			if (toStationLocation == fromStation.StationLocation.BluewardStationLocation())
				return fromStation.PerformMoveOutTowardsBlue(performingPlayer, currentTurn);
			throw new InvalidOperationException("Could not find a path to heroically move out of current station");
		}

		private static bool CanMoveOut(StandardStation fromStation, StationLocation toStationLocation)
		{
			if (toStationLocation == fromStation.StationLocation.OppositeStationLocation())
				return StandardStation.CanMoveOutTowardsOppositeDeck();
			if (toStationLocation == fromStation.StationLocation.RedwardStationLocation())
				return fromStation.CanMoveOutTowardsRed();
			if (toStationLocation == fromStation.StationLocation.BluewardStationLocation())
				return fromStation.CanMoveOutTowardsBlue();
			throw new InvalidOperationException("Could not determine if player could heroically move out of current station");
		}

		internal static IEnumerable<StationLocation> Path(StationLocation fromLocation, StationLocation toLocation)
		{
			var oppositeDeckPath = OppositeDeckPath(fromLocation, toLocation);
			if (oppositeDeckPath != null)
				return oppositeDeckPath;
			var bluewardPath = BluewardPath(fromLocation, toLocation);
			if (bluewardPath != null)
				return bluewardPath;
			var redWardPath = RedwardPath(fromLocation, toLocation);
			if (redWardPath != null)
				return redWardPath;
			throw new InvalidOperationException("Could not find a path for heroic movement.");
		}

		private static IList<StationLocation> BluewardPath(StationLocation fromLocation, StationLocation toLocation)
		{
			var bluewardLocation = fromLocation.BluewardStationLocation();
			if (bluewardLocation == null)
				return null;
			if (bluewardLocation == toLocation)
				return new List<StationLocation> {toLocation};
			var oppositeDeckPath = OppositeDeckPath(bluewardLocation.Value, toLocation);
			if (oppositeDeckPath != null)
				return new List<StationLocation> {bluewardLocation.Value, oppositeDeckPath.Single()};
			var bluewardPath = BluewardPath(bluewardLocation.Value, toLocation);
			return bluewardPath == null ? null : new [] {bluewardLocation.Value}.Concat(BluewardPath(bluewardLocation.Value, toLocation)).ToList();
		}

		private static IList<StationLocation> RedwardPath(StationLocation fromLocation, StationLocation toLocation)
		{
			var redwardLocation = fromLocation.RedwardStationLocation();
			if (redwardLocation == null)
				return null;
			if (redwardLocation == toLocation)
				return new List<StationLocation> { toLocation };
			var oppositeDeckPath = OppositeDeckPath(redwardLocation.Value, toLocation);
			if (oppositeDeckPath != null)
				return new List<StationLocation> { redwardLocation.Value, oppositeDeckPath.Single() };
			var redwardPath = RedwardPath(redwardLocation.Value, toLocation);
			return redwardPath == null ? null : new[] { redwardLocation.Value }.Concat(RedwardPath(redwardLocation.Value, toLocation)).ToList();
		}

		private static IEnumerable<StationLocation> OppositeDeckPath(StationLocation fromLocation, StationLocation toLocation)
		{
			if (toLocation == fromLocation.OppositeStationLocation())
				return new List<StationLocation> { toLocation };
			return null;
		}
	}
}
