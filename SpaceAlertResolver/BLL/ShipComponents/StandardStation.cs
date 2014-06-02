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
			if (firstBThreat != null)
				DamageThreat(firstBThreat, performingPlayer);
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.B))
				EnergyContainer.PerformBAction();
		}

		public override PlayerDamage PerformAAction(Player performingPlayer, int currentTurn)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A);
			if (firstAThreat != null)
			{
				DamageThreat(firstAThreat, performingPlayer);
				return null;
			}
			return !HasIrreparableMalfunctionOfType(PlayerAction.A) ? Cannon.PerformAAction() : null;
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C);
			if (firstCThreat != null)
				DamageThreat(firstCThreat, performingPlayer);
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.C))
				CComponent.PerformCAction(performingPlayer);
		}

		public override void UseBattleBots(Player performingPlayer)
		{
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			if (firstBattleBotThreat != null)
				DamageThreat(firstBattleBotThreat, performingPlayer);
		}
	}
}
