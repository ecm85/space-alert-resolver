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

		public virtual void PerformEndOfTurn()
		{
			Cannon.PerformEndOfTurn();
		}

		private bool HasIrreparableMalfunctionOfType(PlayerActionType playerActionType)
		{
			return IrreparableMalfunctions.Any(malfunction => malfunction.ActionType == playerActionType);
		}

		private void PerformBAction(Player performingPlayer, bool isHeroic, bool isRemote = false)
		{
			var firstBThreat = GetFirstThreatOfType(PlayerActionType.B, performingPlayer);
			if (firstBThreat != null)
			{
				if (!(isRemote && firstBThreat.NextDamageWillDestroyThreat()))
					DamageThreat(isHeroic ? 2 : 1, firstBThreat, performingPlayer, isHeroic);
			}
			else if (!HasIrreparableMalfunctionOfType(PlayerActionType.B))
				RefillEnergy(isHeroic);
		}

		private PlayerDamage[] PerformAAction(Player performingPlayer, bool isHeroic, bool isAdvanced = false)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerActionType.A, performingPlayer);
			if (firstAThreat != null)
			{
				DamageThreat(isHeroic ? 2 : 1, firstAThreat, performingPlayer, isHeroic);
				Cannon.RemoveMechanicBuff();
				return null;
			}
			if (HasIrreparableMalfunctionOfType(PlayerActionType.A))
			{
				Cannon.RemoveMechanicBuff();
				return null;
			}
			return Cannon.PerformAAction(isHeroic, performingPlayer, isAdvanced);
		}

		private bool CanFireCannon(Player performingPlayer)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerActionType.A, performingPlayer);
			return firstAThreat == null && !HasIrreparableMalfunctionOfType(PlayerActionType.A) && Cannon.CanFire();
		}

		private void PerformCAction(Player performingPlayer, int currentTurn, bool isRemote = false, bool isAdvanced = false)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerActionType.C, performingPlayer);
			if (firstCThreat != null)
			{
				if (!(isRemote && firstCThreat.NextDamageWillDestroyThreat()))
					DamageThreat(1, firstCThreat, performingPlayer, false);
			}
			else if (!HasIrreparableMalfunctionOfType(PlayerActionType.C))
				CComponent.PerformCAction(performingPlayer, currentTurn, isAdvanced);
		}

		private bool CanUseCComponent(Player performingPlayer)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerActionType.C, performingPlayer);
			return firstCThreat == null && !HasIrreparableMalfunctionOfType(PlayerActionType.C) && CComponent.CanPerformCAction(performingPlayer);
		}

		private void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			if (performingPlayer.BattleBots == null || performingPlayer.BattleBots.IsDisabled)
				return;
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerActionType.BattleBots, performingPlayer);
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

		public override PlayerDamage[] PerformPlayerAction(Player player, int currentTurn)
		{
			switch (player.Actions[currentTurn].ActionType)
			{
				case PlayerActionType.A:
					return PerformAAction(player, false);
				case PlayerActionType.B:
					PerformBAction(player, false);
					break;
				case PlayerActionType.C:
					PerformCAction(player, currentTurn);
					break;
				case PlayerActionType.MoveBlue:
					MovementController.MoveBlue(player, currentTurn);
					break;
				case PlayerActionType.MoveRed:
					MovementController.MoveRed(player, currentTurn);
					break;
				case PlayerActionType.ChangeDeck:
					MovementController.ChangeDeck(player, currentTurn);
					break;
				case PlayerActionType.BattleBots:
					UseBattleBots(player, false);
					break;
				case PlayerActionType.HeroicA:
					return PerformAAction(player, true);
				case PlayerActionType.HeroicB:
					PerformBAction(player, true);
					break;
				case PlayerActionType.HeroicBattleBots:
					UseBattleBots(player, true);
					break;
				case PlayerActionType.TeleportBlueLower:
					MovementController.MoveHeroically(player, StationLocation.LowerBlue, currentTurn);
					break;
				case PlayerActionType.TeleportBlueUpper:
					MovementController.MoveHeroically(player, StationLocation.UpperBlue, currentTurn);
					break;
				case PlayerActionType.TeleportWhiteLower:
					MovementController.MoveHeroically(player, StationLocation.LowerWhite, currentTurn);
					break;
				case PlayerActionType.TeleportWhiteUpper:
					MovementController.MoveHeroically(player, StationLocation.UpperWhite, currentTurn);
					break;
				case PlayerActionType.TeleportRedLower:
					MovementController.MoveHeroically(player, StationLocation.LowerRed, currentTurn);
					break;
				case PlayerActionType.TeleportRedUpper:
					MovementController.MoveHeroically(player, StationLocation.UpperRed, currentTurn);
					break;
				case PlayerActionType.BasicSpecialization:
					return PerformBasicSpecialization(player, currentTurn);
				case PlayerActionType.AdvancedSpecialization:
					return PerformAdvancedSpecialization(player, currentTurn);
			}
			return null;
		}

		private PlayerDamage[] PerformBasicSpecialization(Player player, int currentTurn)
		{
			switch (player.BasicSpecialization)
			{
				case PlayerSpecialization.DataAnalyst:
					SittingDuck.StandardStationsByLocation[StationLocation.UpperWhite].PerformCAction(player, currentTurn, true);
					break;
				case PlayerSpecialization.EnergyTechnician:
					SittingDuck.StandardStationsByLocation[StationLocation.LowerWhite].PerformBAction(player, false, true);
					break;
				case PlayerSpecialization.Hypernavigator:
					if (StationLocation.IsLowerDeck())
					{
						SittingDuck.ThreatController.AddExternalThreatEffect(ExternalThreatEffect.ReducedMovement, player);
						SittingDuck.ThreatController.EndOfTurn += RestoreThreatMovement;
					}
					break;
				case PlayerSpecialization.Mechanic:
					Cannon.AddMechanicBuff();
					break;
				case PlayerSpecialization.Medic:
					throw new NotImplementedException();
				case PlayerSpecialization.PulseGunner:
					var pulseCannonStation = SittingDuck.StandardStationsByLocation[StationLocation.LowerWhite];
					if (CanFireCannon(player) && pulseCannonStation.CanFireCannon(player) && StationLocation != StationLocation.LowerWhite)
						return PerformAAction(player, false).Concat(pulseCannonStation.PerformAAction(player, false)).ToArray();
					if (StationLocation == StationLocation.LowerWhite)
						return PerformAAction(player, false);
					pulseCannonStation.Cannon.RemoveMechanicBuff();
					if(!CanFireCannon(player))
						return PerformAAction(player, false);
					Cannon.RemoveMechanicBuff();
					break;
				case PlayerSpecialization.Rocketeer:
					SittingDuck.StandardStationsByLocation[StationLocation.LowerBlue].PerformCAction(player, currentTurn, true);
					break;
				case PlayerSpecialization.SpecialOps:
					var indexOfNextActionToMakeHeroic = player.Actions.FindIndex(currentTurn + 1, action => action.ActionType.CanBeMadeHeroic());
					player.Actions[indexOfNextActionToMakeHeroic].ActionType = player.Actions[indexOfNextActionToMakeHeroic].ActionType.MakeHeroic();
					break;
				case PlayerSpecialization.SquadLeader:
					if (player.BattleBots != null)
					{
						if (player.BattleBots.IsDisabled)
							player.BattleBots.IsDisabled = false;
						else
							SittingDuck.ZonesByLocation[StationLocation.ZoneLocation()].RepairFirstDamage(player);
					}
					break;
				case PlayerSpecialization.Teleporter:
					var playerToTeleport = SittingDuck.GetPlayersOnShip().SingleOrDefault(playerOnShip => playerOnShip.PlayerToTeleport);
					var teleportDestination = SittingDuck.GetPlayersOnShip().SingleOrDefault(playerOnShip => playerOnShip.TeleportDestination);
					if(playerToTeleport != null && teleportDestination != null)
						playerToTeleport.CurrentStation = teleportDestination.CurrentStation;
					break;
				default:
					throw new InvalidOperationException("Missing specialization when attempting basic specialization.");
			}
			return null;
		}

		private void RestoreThreatMovement()
		{
			SittingDuck.ThreatController.EndOfTurn -= RestoreThreatMovement;
			SittingDuck.ThreatController.RemoveExternalThreatEffectForSource(this);
		}

		private PlayerDamage[] PerformAdvancedSpecialization(Player player, int currentTurn)
		{
			switch (player.BasicSpecialization)
			{
				case PlayerSpecialization.DataAnalyst:
					PerformCAction(player, currentTurn, false, StationLocation == StationLocation.LowerWhite);
					player.BonusPoints++;
					break;
				case PlayerSpecialization.EnergyTechnician:
					if (player.CurrentStation.StationLocation.IsUpperDeck())
						foreach (var zone in SittingDuck.Zones)
						{
							var isCurrentZone = (player.CurrentStation.StationLocation.ZoneLocation() == zone.ZoneLocation);
							zone.UpperStation.Shield.BonusShield += isCurrentZone ? 2 : 1;
						}
					break;
				case PlayerSpecialization.Hypernavigator:
					if (currentTurn == 9 || currentTurn == 10)
						SittingDuck.Game.NumberOfTurns = currentTurn + 1;
					break;
				case PlayerSpecialization.Mechanic:
					var firstThreat = new[] {PlayerActionType.A, PlayerActionType.B, PlayerActionType.C}
						.Select(actionType => GetFirstThreatOfType(actionType, player))
						.FirstOrDefault(threat => threat != null);
					if(firstThreat != null)
						DamageThreat(2, firstThreat, player, false);
					break;
				case PlayerSpecialization.Medic:
					throw new NotImplementedException();
				case PlayerSpecialization.PulseGunner:
					return PerformAAction(player, false, StationLocation == StationLocation.LowerWhite);
				case PlayerSpecialization.Rocketeer:
					PerformCAction(player, currentTurn, false, StationLocation == StationLocation.LowerBlue);
					break;
				case PlayerSpecialization.SpecialOps:
					throw new NotImplementedException();
				case PlayerSpecialization.SquadLeader:
					var canGoIntoSpace = player.BattleBots != null &&
						!player.BattleBots.IsDisabled &&
						player.CurrentStation.StationLocation != StationLocation.LowerBlue;
					if (canGoIntoSpace)
					{
						MovementController.MoveHeroically(player, StationLocation.UpperRed, currentTurn);
						var newStation = SittingDuck.StandardStationsByLocation[player.CurrentStation.StationLocation];
						if(newStation.StationLocation == StationLocation.UpperRed && newStation.CanUseCComponent(player))
							newStation.PerformCAction(player, currentTurn);
					}
					break;
				case PlayerSpecialization.Teleporter:
					var newStationLocation = StationLocation.DiagonalStation();
					if (newStationLocation != null)
						player.CurrentStation = SittingDuck.StandardStationsByLocation[newStationLocation.Value];
					break;
				default:
					throw new InvalidOperationException("Missing specialization when attempting advanced specialization.");
			}
			return null;
		}
	}
}
