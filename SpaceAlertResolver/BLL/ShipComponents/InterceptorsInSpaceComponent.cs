using BLL.Common;
using BLL.Players;

namespace BLL.ShipComponents
{
	public class InterceptorsInSpaceComponent : ICharlieComponent
	{
		private readonly SittingDuck sittingDuck;
		private readonly StationLocation stationLocation;

		private Station SpacewardStation
		{
			get { return sittingDuck.StationsByLocation[stationLocation.SpacewardLocation().GetValueOrDefault()]; }
		}

		private Station ShipwardLocation
		{
			get { return sittingDuck.StationsByLocation[stationLocation.ShipwardLocation().GetValueOrDefault()]; }
		}

		internal InterceptorsInSpaceComponent(
			SittingDuck sittingDuck,
			StationLocation stationLocation)
		{
			this.stationLocation = stationLocation;
			this.sittingDuck = sittingDuck;
		}

		public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");

			var currentDistanceFromShip = performingPlayer.CurrentStation.StationLocation.DistanceFromShip();
			if (currentDistanceFromShip == null || currentDistanceFromShip < 3)
			{
				performingPlayer.CurrentStation.Players.Remove(performingPlayer);
				SpacewardStation.MovePlayerIn(performingPlayer, currentTurn);
			}
			else
			{
				performingPlayer.KnockOutFromOwnAction();
			}
		}

		public bool CanPerformCAction(Player performingPlayer)
		{
			return performingPlayer.Interceptors != null;
		}

		public void PerformNoAction(Player performingPlayer, int currentTurn)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			if (ShipwardLocation != null && performingPlayer.Interceptors != null)
			{
				performingPlayer.CurrentStation.Players.Remove(performingPlayer);
				ShipwardLocation.MovePlayerIn(performingPlayer, currentTurn);
			}
		}
	}
}
