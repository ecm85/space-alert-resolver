using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public class StandardStation : Station
	{
		public Gravolift Gravolift { get; set; }
		public Airlock BluewardAirlock { get; set; }
		public Airlock RedwardAirlock { get; set; }
		public Cannon Cannon { get; set; }
		public IList<IrreparableMalfunction> IrreparableMalfunctions { get; private set; }

		public StandardStation()
		{
			IrreparableMalfunctions = new List<IrreparableMalfunction>();
		}

		protected bool HasIrreparableMalfunctionOfType(PlayerAction playerAction)
		{
			return IrreparableMalfunctions.Any(malfunction => malfunction.ActionType == playerAction);
		}

		public override void PerformBAction(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			var firstBThreat = GetFirstThreatOfType(PlayerAction.B);
			if (firstBThreat != null)
				DamageThreat(firstBThreat, performingPlayer, isHeroic);
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.B))
				EnergyContainer.PerformBAction(isHeroic);
		}

		public override PlayerDamage PerformAAction(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A);
			if (firstAThreat != null)
			{
				DamageThreat(firstAThreat, performingPlayer, isHeroic);
				return null;
			}
			return !HasIrreparableMalfunctionOfType(PlayerAction.A) ? Cannon.PerformAAction(isHeroic) : null;
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C);
			if (firstCThreat != null)
				DamageThreat(firstCThreat, performingPlayer, false);
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.C))
				CComponent.PerformCAction(performingPlayer);
		}

		public override void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			if (firstBattleBotThreat != null)
				DamageThreat(firstBattleBotThreat, performingPlayer, isHeroic);
		}

		public override bool PerformMoveOutTowardsRed(Player performingPlayer, int currentTurn)
		{
			if (!CanMoveOutTowardsRed())
				return false;
			OnMoveOut(performingPlayer, currentTurn);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public override bool PerformMoveOutTowardsOppositeDeck(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			OnMoveOut(performingPlayer, currentTurn);
			Gravolift.Use(performingPlayer, currentTurn, isHeroic);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public override bool PerformMoveOutTowardsBlue(Player performingPlayer, int currentTurn)
		{
			if (!CanMoveOutTowardsBlue())
				return false;
			OnMoveOut(performingPlayer, currentTurn);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public override void PerformMoveIn(Player performingPlayer, int currentTurn)
		{
			OnMoveIn(performingPlayer, currentTurn);
			Players.Add(performingPlayer);
			performingPlayer.CurrentStation = this;
		}

		public override bool CanMoveOutTowardsRed()
		{
			return RedwardAirlock != null && RedwardAirlock.CanUse;
		}

		public override bool CanMoveOutTowardsOppositeDeck()
		{
			return true;
		}

		public override bool CanMoveOutTowardsBlue()
		{
			return BluewardAirlock != null && BluewardAirlock.CanUse;
		}
	}
}
