using System.Collections.Generic;
using System.Linq;
using BLL.Common;

namespace BLL.ShipComponents
{
	public class InterceptorStation : Station
	{
		public InterceptorsInSpaceComponent InterceptorComponent { get; private set; }
		public PlayerInterceptorDamage PlayerInterceptorDamage { get; private set; }

		public InterceptorStation(
			StationLocation stationLocation,
			ThreatController threatController,
			InterceptorsInSpaceComponent interceptorComponent) : base(stationLocation, threatController)
		{
			MovingIn += UseBattleBots;
			InterceptorComponent = interceptorComponent;
		}

		private void PerformCAction(Player performingPlayer, int currentTurn)
		{
			InterceptorComponent.PerformCAction(performingPlayer, currentTurn, false);
		}

		private void UseBattleBots(object sender, PlayerMoveEventArgs eventArgs)
		{
			UseBattleBots(eventArgs.MovingPlayer, false);
		}

		private void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			var firstThreat = GetFirstThreatOfType(PlayerActionType.BattleBots, performingPlayer);
			if (firstThreat == null)
				PlayerInterceptorDamage = new PlayerInterceptorDamage(isHeroic, performingPlayer, StationLocation.DistanceFromShip().GetValueOrDefault());
			else
				DamageThreat(1, firstThreat, performingPlayer, isHeroic);
		}

		public override void MovePlayerIn(Player performingPlayer, int? currentTurn = null)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			Players.Add(performingPlayer);
			performingPlayer.CurrentStation = this;
			OnPlayerMovingIn(performingPlayer, currentTurn);
		}

		public override void PerformNextPlayerAction(Player performingPlayer, int currentTurn)
		{
			//TODO: Perform bonus action
			var playerAction = performingPlayer.GetActionForTurn(currentTurn);
			if (playerAction.FirstActionPerformed)
			{
				playerAction.SecondActionPerformed = true;
				playerAction.BonusActionPerformed = true;
				return;
			}
			PerformPlayerAction(performingPlayer, GetActionPerformedInSpace(playerAction), currentTurn);
		}

		private static PlayerActionType? GetActionPerformedInSpace(PlayerAction action)
		{
			var actionPriority = new Queue<PlayerActionType?>(new List<PlayerActionType?>
			{
				PlayerActionType.Charlie,
				PlayerActionType.BattleBots,
				PlayerActionType.HeroicBattleBots,
				PlayerActionType.AdvancedSpecialization
			});
			while (actionPriority.Any())
			{
				var nextActionPriority = actionPriority.Dequeue();
				if (action.FirstActionType == nextActionPriority || action.SecondActionType == nextActionPriority)
					return nextActionPriority;
			}
			return action.FirstActionType ?? action.SecondActionType;
		}

		private void PerformPlayerAction(Player performingPlayer, PlayerActionType? playerActionType, int currentTurn)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			switch (playerActionType)
			{
				case PlayerActionType.Charlie:
					PerformCAction(performingPlayer, currentTurn);
					break;
				case PlayerActionType.BattleBots:
					UseBattleBots(performingPlayer, false);
					break;
				case PlayerActionType.HeroicBattleBots:
					UseBattleBots(performingPlayer, true);
					break;
				case PlayerActionType.AdvancedSpecialization:
					if (performingPlayer.Specialization == PlayerSpecialization.SquadLeader)
						UseBattleBots(performingPlayer, true);
					else
						PerformInvalidAction(performingPlayer, currentTurn);
					break;
				default:
					PerformInvalidAction(performingPlayer, currentTurn);
					break;
			}
			var playerAction = performingPlayer.GetActionForTurn(currentTurn);
			playerAction.FirstActionPerformed = true;
			playerAction.SecondActionPerformed = true;
			playerAction.BonusActionPerformed = true;
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
