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

		public InterceptorStation()
		{
			MoveIn += UseBattleBots;
		}

		public override void PerformBAction(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer, currentTurn);
		}

		public override PlayerDamage PerformAAction(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer, currentTurn);
			return null;
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			InterceptorComponent.PerformCAction(performingPlayer, currentTurn);
		}

		private void UseBattleBots(Player performingPlayer, int currentTurn)
		{
			UseBattleBots(performingPlayer, currentTurn, false);
		}

		public override void UseBattleBots(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			var firstThreat = GetFirstThreatOfType(PlayerAction.BattleBots, performingPlayer);
			if (firstThreat == null)
				PlayerInterceptorDamage = new PlayerInterceptorDamage(isHeroic, performingPlayer, StationLocation.DistanceFromShip().GetValueOrDefault());
			else
				DamageThreat(1, firstThreat, performingPlayer, isHeroic);
		}

		public override bool PerformMoveOutTowardsRed(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer, currentTurn);
			return false;
		}

		public override bool PerformMoveOutTowardsOppositeDeck(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer, currentTurn);
			return false;
		}

		public override bool PerformMoveOutTowardsBlue(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer, currentTurn);
			return false;
		}

		public override void PerformMoveIn(Player performingPlayer, int currentTurn)
		{
			Players.Add(performingPlayer);
			performingPlayer.CurrentStation = this;
			OnMoveIn(performingPlayer, currentTurn);
		}

		public override bool CanMoveOutTowardsRed()
		{
			return false;
		}

		public override bool CanMoveOutTowardsOppositeDeck()
		{
			return false;
		}

		public override bool CanMoveOutTowardsBlue()
		{
			return false;
		}

		public override void PerformNoAction(Player performingPlayer, int currentTurn)
		{
			InterceptorComponent.PerformNoAction(performingPlayer, currentTurn);
		}

		public override void DrainEnergyContainer(int amount)
		{
			throw new InvalidOperationException();
		}

		public void PerformEndOfTurn()
		{
			PlayerInterceptorDamage = null;
		}
	}
}
