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
		public Interceptors Interceptors { get; set; }
		public InterceptorComponent(InterceptorStation spacewardStation, Station shipwardStation, Interceptors interceptors)
		{
			this.spacewardStation = spacewardStation;
			this.shipwardStation = shipwardStation;
			Interceptors = interceptors;
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn, bool advancedUsage = false)
		{
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
					spacewardStation.PerformMoveIn(performingPlayer, currentTurn);
				}
				else
				{
					performingPlayer.IsKnockedOut = true;
					if (performingPlayer.BattleBots != null)
						performingPlayer.BattleBots.IsDisabled = true;
				}
			}
		}

		public void PerformNoAction(Player performingPlayer, int currentTurn)
		{
			if (shipwardStation != null && performingPlayer.Interceptors != null)
			{
				performingPlayer.CurrentStation.Players.Remove(performingPlayer);
				shipwardStation.PerformMoveIn(performingPlayer, currentTurn);
			}
		}
	}
}
