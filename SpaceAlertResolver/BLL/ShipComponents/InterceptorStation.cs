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

		public override EnergyContainer EnergyContainer
		{
			get { throw new InvalidOperationException("Interceptor station has no energy container."); }
			set { throw new InvalidOperationException("Interceptor station has no energy container."); }
		}

		public override void PerformBAction(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			performingPlayer.Shift(currentTurn);
			InterceptorComponent.PerformNoAction(performingPlayer);
		}

		public override PlayerDamage PerformAAction(Player performingPlayer, int currentTurn, bool isHeroic)
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

		public override void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			UseInterceptors(performingPlayer, isHeroic);
		}

		public override bool PerformMoveOutTowardsRed(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			return false;
		}

		public override bool PerformMoveOutTowardsOppositeDeck(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			performingPlayer.Shift(currentTurn);
			return false;
		}

		public override bool PerformMoveOutTowardsBlue(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			return false;
		}

		public override void PerformMoveIn(Player performingPlayer)
		{
			throw new InvalidOperationException("Cannot move to interceptor station by normal means");
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

		public override void UseInterceptors(Player performingPlayer, bool isHeroic)
		{
			var firstThreat = GetFirstThreatOfType(PlayerAction.BattleBots);
			if (firstThreat == null)
				PlayerInterceptorDamage = new PlayerInterceptorDamage(isHeroic);
			else
				DamageThreat(firstThreat, performingPlayer, isHeroic);
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
