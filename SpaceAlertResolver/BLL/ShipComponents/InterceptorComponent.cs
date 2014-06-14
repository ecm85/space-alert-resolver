using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class InterceptorComponent : CComponent
	{
		private readonly Station spacewardStation;
		private readonly Station shipwardStation;
		public InterceptorComponent(Station spacewardStation, Station shipwardStation)
		{
			this.spacewardStation = spacewardStation;
			this.shipwardStation = shipwardStation;
		}

		//Only legal to call from a regular station, or from an interceptor station if variable range interceptors are allowed
		public override void PerformCAction(Player performingPlayer)
		{
			if (performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled && !spacewardStation.Players.Any())
			{
				if (spacewardStation != null)
				{
					performingPlayer.CurrentStation = spacewardStation;
					spacewardStation.UseInterceptors(performingPlayer, false);
				}
				else
				{
					//TODO: Player returns to ship and knocked out
				}
			}
		}

		public void PerformNoAction(Player performingPlayer, int currentTurn)
		{
			if (performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled && shipwardStation != null)
			{
				performingPlayer.CurrentStation.Players.Remove(performingPlayer);
				shipwardStation.PerformMoveIn(performingPlayer, currentTurn);
				shipwardStation.UseInterceptors(performingPlayer, false);
			}
		}
	}
}
