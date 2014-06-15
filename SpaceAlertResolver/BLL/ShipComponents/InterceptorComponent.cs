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
		public InterceptorComponent(InterceptorStation spacewardStation, Station shipwardStation)
		{
			this.spacewardStation = spacewardStation;
			this.shipwardStation = shipwardStation;
		}

		//TODO: VR Interceptors: Only allow one person in space at a time, not just in the destination zone
		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			if (performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled && !spacewardStation.Players.Any())
			{
				var currentDistanceFromShip = performingPlayer.CurrentStation.StationLocation.DistanceFromShip();
				if (currentDistanceFromShip == null || currentDistanceFromShip < 3)
				{
					performingPlayer.CurrentStation.Players.Remove(performingPlayer);
					spacewardStation.PerformMoveIn(performingPlayer, currentTurn);
				}
				else
				{
					//TODO: VR Interceptors: Player returns to ship and knocked out
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
