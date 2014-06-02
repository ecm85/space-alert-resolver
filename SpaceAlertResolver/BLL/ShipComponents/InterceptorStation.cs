using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class InterceptorStation : Station
	{
		public InterceptorComponent InterceptorComponent { private get; set; }
		public PlayerInterceptorDamage PlayerInterceptorDamage { get; private set; }

		public override void PerformBAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer);
		}

		public override PlayerDamage PerformAAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer);
			return null;
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer);
			//TODO: Change to a further interceptor station instead, if variable range interceptors are in use
		}

		public override void UseBattleBots(Player performingPlayer)
		{
			UseInterceptors(performingPlayer);
		}

		public override void UseInterceptors(Player performingPlayer)
		{
			var firstThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			if (firstThreat == null)
				PlayerInterceptorDamage = new PlayerInterceptorDamage();
			else
				DamageThreat(firstThreat, performingPlayer);
		}

		public override void PerformNoAction(Player performingPlayer)
		{
			InterceptorComponent.PerformNoAction(performingPlayer);
		}

		public void PerformEndOfTurn()
		{
			PlayerInterceptorDamage = null;
		}
	}
}
