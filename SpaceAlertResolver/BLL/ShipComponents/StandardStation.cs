using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class StandardStation : Station
	{
		public override void PerformBAction(Player performingPlayer, int currentTurn)
		{
			var firstBThreat = GetFirstThreatOfType(PlayerAction.B);
			if (firstBThreat == null)
				EnergyContainer.PerformBAction();
			else
				DamageThreat(firstBThreat, performingPlayer);
		}

		public override PlayerDamage PerformAAction(Player performingPlayer, int currentTurn)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A);
			if (firstAThreat == null)
				return Cannon.PerformAAction();
			DamageThreat(firstAThreat, performingPlayer);
			return null;
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C);
			if (firstCThreat == null)
				CComponent.PerformCAction(performingPlayer);
			else
				DamageThreat(firstCThreat, performingPlayer);
		}

		public override void UseBattleBots(Player performingPlayer)
		{
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			if (firstBattleBotThreat != null)
				DamageThreat(firstBattleBotThreat, performingPlayer);
		}
	}
}
