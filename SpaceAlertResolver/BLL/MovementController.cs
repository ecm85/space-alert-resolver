using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class MovementController
	{
		public SittingDuck SittingDuck { get; set; }

		//TODO: Make poison and delay effects actually happen

		public void ChangeDeck(Player player, int currentTurn)
		{
			var newStation = SittingDuck.StationsByLocation[player.CurrentStation.StationLocation.OppositeStationLocation().GetValueOrDefault()];
			if (newStation != null)
			{
				var movedOut = player.CurrentStation.PerformMoveOutTowardsOppositeDeck(player, currentTurn, false);
				if (movedOut)
					newStation.PerformMoveIn(player);
			}
		}

		public void MoveBlue(Player player, int currentTurn)
		{
			var newStation = SittingDuck.StationsByLocation[player.CurrentStation.StationLocation.BluewardStationLocation().GetValueOrDefault()];
			if (newStation != null)
			{
				var movedOut = player.CurrentStation.PerformMoveOutTowardsBlue(player, currentTurn);
				if (movedOut)
					newStation.PerformMoveIn(player);
			}
		}

		public void MoveRed(Player player, int currentTurn)
		{
			var newStation = SittingDuck.StationsByLocation[player.CurrentStation.StationLocation.RedwardStationLocation().GetValueOrDefault()];
			if (newStation != null)
			{
				var movedOut = player.CurrentStation.PerformMoveOutTowardsRed(player, currentTurn);
				if (movedOut)
					newStation.PerformMoveIn(player);
			}
		}

		public void MoveHeroically(Player performingPlayer, StationLocation newStationLocation, int currentTurn)
		{
			var currentStationLocation = performingPlayer.CurrentStation.StationLocation;
			var path = Path(currentStationLocation, newStationLocation)
				.Select(stationLocation => SittingDuck.StationsByLocation[stationLocation])
				.Select((station, index) => new {Station = station, Index = index})
				.ToList();
			var firstDestination = path.First();
			var movedOut = MoveOut(performingPlayer, currentTurn, performingPlayer.CurrentStation, firstDestination.Station.StationLocation);
			if (!movedOut)
				return;
			var finalDestination = path.First(
				pathLocation => pathLocation.Index >= path.Count - 1 ||
				!CanMoveOut(pathLocation.Station, path[pathLocation.Index + 1].Station.StationLocation));
			finalDestination.Station.PerformMoveIn(performingPlayer);
		}

		private static bool MoveOut(
			Player performingPlayer,
			int currentTurn,
			Station fromStation,
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

		private static bool CanMoveOut(Station fromStation, StationLocation toStationLocation)
		{
			if (toStationLocation == fromStation.StationLocation.OppositeStationLocation())
				return fromStation.CanMoveOutTowardsOppositeDeck();
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
