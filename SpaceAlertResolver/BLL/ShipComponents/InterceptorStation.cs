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

		private void PerformCAction(Player performingPlayer, int currentTurn)
		{
			InterceptorComponent.PerformCAction(performingPlayer, currentTurn);
		}

		private void UseBattleBots(Player performingPlayer, int currentTurn)
		{
			UseBattleBots(performingPlayer, false);
		}

		private void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			var firstThreat = GetFirstThreatOfType(PlayerAction.BattleBots, performingPlayer);
			if (firstThreat == null)
				PlayerInterceptorDamage = new PlayerInterceptorDamage(isHeroic, performingPlayer, StationLocation.DistanceFromShip().GetValueOrDefault());
			else
				DamageThreat(1, firstThreat, performingPlayer, isHeroic);
		}

		public override void PerformMoveIn(Player performingPlayer, int currentTurn)
		{
			Players.Add(performingPlayer);
			performingPlayer.CurrentStation = this;
			OnMoveIn(performingPlayer, currentTurn);
		}

		public override PlayerDamage[] PerformPlayerAction(Player player, PlayerAction action, int currentTurn)
		{
			switch (action)
			{
				case PlayerAction.C:
					PerformCAction(player, currentTurn);
					break;
				case PlayerAction.BattleBots:
					UseBattleBots(player, false);
					break;
				case PlayerAction.HeroicBattleBots:
					UseBattleBots(player, true);
					break;
				case PlayerAction.AdvancedSpecialization:
					if (player.AdvancedSpecialization == PlayerSpecialization.SquadLeader)
						UseBattleBots(player, true);
					else
						PerformInvalidAction(player, currentTurn);
					break;
				default:
					PerformInvalidAction(player, currentTurn);
					break;
			}
			return null;
		}

		private void PerformInvalidAction(Player player, int currentTurn)
		{
			InterceptorComponent.PerformNoAction(player, currentTurn);
			player.Shift(currentTurn);
		}

		public void PerformEndOfTurn()
		{
			PlayerInterceptorDamage = null;
		}
	}
}
