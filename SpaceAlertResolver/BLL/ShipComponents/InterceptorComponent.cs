using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class InterceptorComponent : CComponent
	{
		private readonly InterceptorStation spacewardStation;
		private readonly Station shipwardStation;
		public bool InterceptorsAvailable { get; set; }
		public InterceptorComponent(InterceptorStation spacewardStation, Station shipwardStation, bool interceptorsAvailable = false)
		{
			this.spacewardStation = spacewardStation;
			this.shipwardStation = shipwardStation;
			InterceptorsAvailable = interceptorsAvailable;
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			if (performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled && !spacewardStation.Players.Any())
			{
				if (InterceptorsAvailable)
				{
					performingPlayer.IsUsingInterceptors = true;
					InterceptorsAvailable = false;
				}
				if (performingPlayer.IsUsingInterceptors)
				{
					var currentDistanceFromShip = performingPlayer.CurrentStation.StationLocation.DistanceFromShip();
					if (currentDistanceFromShip == null || currentDistanceFromShip < 3)
					{
						performingPlayer.CurrentStation.Players.Remove(performingPlayer);
						spacewardStation.PerformMoveIn(performingPlayer, currentTurn);
					}
					else
					{
						performingPlayer.IsKnockedOut = true;
						performingPlayer.BattleBots.IsDisabled = true;
					}
				}
			}
		}

		public void PerformNoAction(Player performingPlayer, int currentTurn)
		{
			if (performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled && shipwardStation != null)
			{
				performingPlayer.CurrentStation.Players.Remove(performingPlayer);
				shipwardStation.PerformMoveIn(performingPlayer, currentTurn);
			}
		}
	}
}
