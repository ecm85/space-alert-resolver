using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Common;

namespace BLL.ShipComponents
{
	public class InterceptorComponent : ICharlieComponent
	{
		private readonly SittingDuck sittingDuck;
		private readonly StationLocation stationLocation;
		private Interceptors Interceptors { get; set; }

		private Station SpacewardStation
		{
			get { return sittingDuck.StationsByLocation[stationLocation.SpacewardLocation().GetValueOrDefault()]; }
		}

		private Station ShipwardLocation
		{
			get { return sittingDuck.StationsByLocation[stationLocation.ShipwardLocation().GetValueOrDefault()]; }
		}

		public InterceptorComponent(
			SittingDuck sittingDuck,
			Interceptors interceptors,
			StationLocation stationLocation)
		{
			Interceptors = interceptors;
			this.stationLocation = stationLocation;
			this.sittingDuck = sittingDuck;
		}

		public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			if (performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled && Interceptors.PlayerOperating == null)
			{
				Interceptors.PlayerOperating = performingPlayer;
				performingPlayer.Interceptors = Interceptors;
			}

			if (performingPlayer.Interceptors != null)
			{
				var currentDistanceFromShip = performingPlayer.CurrentStation.StationLocation.DistanceFromShip();
				if (currentDistanceFromShip == null || currentDistanceFromShip < 3)
				{
					performingPlayer.CurrentStation.Players.Remove(performingPlayer);
					SpacewardStation.PerformMoveIn(performingPlayer, currentTurn);
				}
				else
				{
					performingPlayer.IsKnockedOut = true;
					if (performingPlayer.BattleBots != null)
						performingPlayer.BattleBots.IsDisabled = true;
				}
			}
		}

		public bool CanPerformCAction(Player performingPlayer)
		{
			return Interceptors.PlayerOperating == null;
		}

		public void PerformNoAction(Player performingPlayer, int currentTurn)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			if (ShipwardLocation != null && performingPlayer.Interceptors != null)
			{
				performingPlayer.CurrentStation.Players.Remove(performingPlayer);
				ShipwardLocation.PerformMoveIn(performingPlayer, currentTurn);
			}
		}
	}
}
