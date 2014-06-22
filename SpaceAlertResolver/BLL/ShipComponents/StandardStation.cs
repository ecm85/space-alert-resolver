using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class StandardStation : Station
	{
		public abstract void DrainEnergyContainer(int amount);

		public Gravolift Gravolift { get; set; }
		public Airlock BluewardAirlock { get; set; }
		public Airlock RedwardAirlock { get; set; }
		public Cannon Cannon { get; set; }
		public MovementController MovementController { get; set; }
		public SittingDuck SittingDuck { get; set; }

		protected abstract void RefillEnergy(bool isHeroic);

		private bool HasIrreparableMalfunctionOfType(PlayerAction playerAction)
		{
			return IrreparableMalfunctions.Any(malfunction => malfunction.ActionType == playerAction);
		}

		private void PerformBAction(Player performingPlayer, bool isHeroic)
		{
			var firstBThreat = GetFirstThreatOfType(PlayerAction.B, performingPlayer);
			if (firstBThreat != null)
				DamageThreat(isHeroic ? 2 : 1, firstBThreat, performingPlayer, isHeroic);
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.B))
				RefillEnergy(isHeroic);
		}

		private PlayerDamage PerformAAction(Player performingPlayer, bool isHeroic)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A, performingPlayer);
			if (firstAThreat != null)
			{
				DamageThreat(isHeroic ? 2 : 1, firstAThreat, performingPlayer, isHeroic);
				return null;
			}
			return !HasIrreparableMalfunctionOfType(PlayerAction.A) ? Cannon.PerformAAction(isHeroic, performingPlayer) : null;
		}

		private void PerformCAction(Player performingPlayer, int currentTurn, bool isRemote = false, bool advanced = false)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C, performingPlayer);
			if (firstCThreat != null)
			{
				if (!(isRemote && firstCThreat.NextDamageWillDestroyThreat()))
					DamageThreat(1, firstCThreat, performingPlayer, false);
			}
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.C))
				CComponent.PerformCAction(performingPlayer, currentTurn, advanced);

		}

		private void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			if (performingPlayer.BattleBots == null || performingPlayer.BattleBots.IsDisabled)
				return;
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerAction.BattleBots, performingPlayer);
			if (firstBattleBotThreat != null)
				DamageThreat(1, firstBattleBotThreat, performingPlayer, isHeroic);
		}

		public bool PerformMoveOutTowardsRed(Player performingPlayer, int currentTurn)
		{
			if (!CanMoveOutTowardsRed())
				return false;
			OnMoveOut(performingPlayer, currentTurn);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public bool PerformMoveOutTowardsOppositeDeck(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			OnMoveOut(performingPlayer, currentTurn);
			Gravolift.Use(performingPlayer, currentTurn, isHeroic);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public bool PerformMoveOutTowardsBlue(Player performingPlayer, int currentTurn)
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
			if (performingPlayer.Interceptors != null)
			{
				performingPlayer.Interceptors.PlayerOperating = null;
				performingPlayer.Interceptors = null;
			}
		}

		public bool CanMoveOutTowardsRed()
		{
			return RedwardAirlock != null && RedwardAirlock.CanUse;
		}

		public bool CanMoveOutTowardsOppositeDeck()
		{
			return true;
		}

		public bool CanMoveOutTowardsBlue()
		{
			return BluewardAirlock != null && BluewardAirlock.CanUse;
		}

		public override PlayerDamage[] PerformPlayerAction(Player player, PlayerAction action, int currentTurn)
		{
			switch (action)
			{
				case PlayerAction.A:
					return new [] {PerformAAction(player, false)};
				case PlayerAction.B:
					PerformBAction(player, false);
					break;
				case PlayerAction.C:
					PerformCAction(player, currentTurn);
					break;
				case PlayerAction.MoveBlue:
					MovementController.MoveBlue(player, currentTurn);
					break;
				case PlayerAction.MoveRed:
					MovementController.MoveRed(player, currentTurn);
					break;
				case PlayerAction.ChangeDeck:
					MovementController.ChangeDeck(player, currentTurn);
					break;
				case PlayerAction.BattleBots:
					UseBattleBots(player, false);
					break;
				case PlayerAction.None:
					break;
				case PlayerAction.HeroicA:
					PerformAAction(player, true);
					break;
				case PlayerAction.HeroicB:
					PerformBAction(player, true);
					break;
				case PlayerAction.HeroicBattleBots:
					UseBattleBots(player, true);
					break;
				case PlayerAction.TeleportBlueLower:
					MovementController.MoveHeroically(player, StationLocation.LowerBlue, currentTurn);
					break;
				case PlayerAction.TeleportBlueUpper:
					MovementController.MoveHeroically(player, StationLocation.UpperBlue, currentTurn);
					break;
				case PlayerAction.TeleportWhiteLower:
					MovementController.MoveHeroically(player, StationLocation.LowerWhite, currentTurn);
					break;
				case PlayerAction.TeleportWhiteUpper:
					MovementController.MoveHeroically(player, StationLocation.UpperWhite, currentTurn);
					break;
				case PlayerAction.TeleportRedLower:
					MovementController.MoveHeroically(player, StationLocation.LowerRed, currentTurn);
					break;
				case PlayerAction.TeleportRedUpper:
					MovementController.MoveHeroically(player, StationLocation.UpperRed, currentTurn);
					break;
				case PlayerAction.BasicSpecialization:
					return PerformBasicSpecialization(player, currentTurn);
				case PlayerAction.AdvancedSpecialization:
					return PerformAdvancedSpecialization(player, currentTurn);
			}
			return null;
		}

		private PlayerDamage[] PerformAdvancedSpecialization(Player player, int currentTurn)
		{
			switch (player.BasicSpecialization)
			{
				case PlayerSpecialization.DataAnalyst:
					break;
				case PlayerSpecialization.EnergyTechnician:
					break;
				case PlayerSpecialization.Hypernavigator:
					break;
				case PlayerSpecialization.Mechanic:
					break;
				case PlayerSpecialization.Medic:
					break;
				case PlayerSpecialization.PulseGunner:
					break;
				case PlayerSpecialization.Rocketeer:
					PerformCAction(player, currentTurn, StationLocation == StationLocation.LowerBlue);
					break;
				case PlayerSpecialization.SpecialOps:
					break;
				case PlayerSpecialization.SquadLeader:
					break;
				case PlayerSpecialization.Teleporter:
					break;
				default:
					throw new InvalidOperationException("Missing specialization when attempting advanced specialization.");
			}
			return null;
		}

		private PlayerDamage[] PerformBasicSpecialization(Player player, int currentTurn)
		{
			switch (player.BasicSpecialization)
			{
				case PlayerSpecialization.DataAnalyst:
					break;
				case PlayerSpecialization.EnergyTechnician:
					break;
				case PlayerSpecialization.Hypernavigator:
					break;
				case PlayerSpecialization.Mechanic:
					break;
				case PlayerSpecialization.Medic:
					break;
				case PlayerSpecialization.PulseGunner:
					break;
				case PlayerSpecialization.Rocketeer:
					SittingDuck.StandardStationsByLocation[player.CurrentStation.StationLocation].PerformCAction(player, currentTurn, false, true);
					break;
				case PlayerSpecialization.SpecialOps:
					break;
				case PlayerSpecialization.SquadLeader:
					break;
				case PlayerSpecialization.Teleporter:
					break;
				default:
					throw new InvalidOperationException("Missing specialization when attempting basic specialization.");
			}
			return null;
		}
	}
}
