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

		private void PerformAAction(Player performingPlayer, bool isHeroic, bool isAdvanced = false)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerActionType.A, performingPlayer);
			if (firstAThreat != null)
				DamageThreat(isHeroic ? 2 : 1, firstAThreat, performingPlayer, isHeroic);
			else if (!HasIrreparableMalfunctionOfType(PlayerActionType.A))
				Cannon.PerformAAction(isHeroic, performingPlayer, isAdvanced);
			Cannon.RemoveMechanicBuff();
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

		public override void PerformPlayerAction(Player performingPlayer, int currentTurn)
		{
			switch (performingPlayer.Actions[currentTurn].ActionType)
			{
				case PlayerActionType.A:
					PerformAAction(performingPlayer, false);
					break;
				case PlayerActionType.B:
					PerformBAction(performingPlayer, false);
					break;
				case PlayerActionType.C:
					PerformCAction(performingPlayer, currentTurn);
					break;
				case PlayerActionType.MoveBlue:
					MovementController.MoveBlue(performingPlayer, currentTurn);
					break;
				case PlayerActionType.MoveRed:
					MovementController.MoveRed(performingPlayer, currentTurn);
					break;
				case PlayerActionType.ChangeDeck:
					MovementController.ChangeDeck(performingPlayer, currentTurn);
					break;
				case PlayerActionType.BattleBots:
					UseBattleBots(performingPlayer, false);
					break;
				case PlayerActionType.HeroicA:
					PerformAAction(performingPlayer, true);
					break;
				case PlayerActionType.HeroicB:
					PerformBAction(performingPlayer, true);
					break;
				case PlayerActionType.HeroicBattleBots:
					UseBattleBots(performingPlayer, true);
					break;
				case PlayerActionType.TeleportBlueLower:
					MovementController.MoveHeroically(performingPlayer, StationLocation.LowerBlue, currentTurn);
					break;
				case PlayerActionType.TeleportBlueUpper:
					MovementController.MoveHeroically(performingPlayer, StationLocation.UpperBlue, currentTurn);
					break;
				case PlayerActionType.TeleportWhiteLower:
					MovementController.MoveHeroically(performingPlayer, StationLocation.LowerWhite, currentTurn);
					break;
				case PlayerActionType.TeleportWhiteUpper:
					MovementController.MoveHeroically(performingPlayer, StationLocation.UpperWhite, currentTurn);
					break;
				case PlayerActionType.TeleportRedLower:
					MovementController.MoveHeroically(performingPlayer, StationLocation.LowerRed, currentTurn);
					break;
				case PlayerActionType.TeleportRedUpper:
					MovementController.MoveHeroically(performingPlayer, StationLocation.UpperRed, currentTurn);
					break;
				case PlayerActionType.BasicSpecialization:
					PerformBasicSpecialization(performingPlayer, currentTurn);
					break;
				case PlayerActionType.AdvancedSpecialization:
					PerformAdvancedSpecialization(performingPlayer, currentTurn);
					break;
			}
			
			if (performingPlayer.IsPerformingBasicMedicWithMovement(currentTurn))
				PerformBasicSpecialization(performingPlayer, currentTurn);
			if(performingPlayer.IsPerformingAdvancedMedicWithMovement(currentTurn))
				PerformAdvancedSpecialization(performingPlayer, currentTurn);
		}

		private void PerformBasicSpecialization(Player performingPlayer, int currentTurn)
		{
			switch (performingPlayer.BasicSpecialization)
			{
				case PlayerSpecialization.DataAnalyst:
					SittingDuck.StandardStationsByLocation[StationLocation.UpperWhite].PerformCAction(performingPlayer, currentTurn, true);
					break;
				case PlayerSpecialization.EnergyTechnician:
					SittingDuck.StandardStationsByLocation[StationLocation.LowerWhite].PerformBAction(performingPlayer, false, true);
					break;
				case PlayerSpecialization.Hypernavigator:
					if (StationLocation.IsLowerDeck())
					{
						SittingDuck.ThreatController.AddExternalThreatEffect(ExternalThreatEffect.ReducedMovement, performingPlayer);
						SittingDuck.ThreatController.EndOfTurn += RestoreThreatMovement;
					}
					break;
				case PlayerSpecialization.Mechanic:
					Cannon.AddMechanicBuff();
					break;
				case PlayerSpecialization.Medic:
					var actionsToMakeHeroic = Players
						.Except(new[] {performingPlayer})
						.Select(player => player.Actions[currentTurn])
						.Where(action => action.ActionType.CanBeMadeHeroic());
					foreach (var action in actionsToMakeHeroic)
						action.ActionType = action.ActionType.MakeHeroic();
					break;
				case PlayerSpecialization.PulseGunner:
					var pulseCannonStation = SittingDuck.StandardStationsByLocation[StationLocation.LowerWhite];
					if (CanFireCannon(performingPlayer) && pulseCannonStation.CanFireCannon(performingPlayer) && StationLocation != StationLocation.LowerWhite)
					{
						PerformAAction(performingPlayer, false);
						pulseCannonStation.PerformAAction(performingPlayer, false);
					}
					else if (StationLocation == StationLocation.LowerWhite)
						PerformAAction(performingPlayer, false);
					else
					{
						pulseCannonStation.Cannon.RemoveMechanicBuff();
						if (!CanFireCannon(performingPlayer))
							PerformAAction(performingPlayer, false);
						else
							Cannon.RemoveMechanicBuff();
					}
					break;
				case PlayerSpecialization.Rocketeer:
					SittingDuck.StandardStationsByLocation[StationLocation.LowerBlue].PerformCAction(performingPlayer, currentTurn, true);
					break;
				case PlayerSpecialization.SpecialOps:
					var indexOfNextActionToMakeHeroic = performingPlayer.Actions.FindIndex(currentTurn + 1, action => action.ActionType.CanBeMadeHeroic());
					performingPlayer.Actions[indexOfNextActionToMakeHeroic].ActionType = performingPlayer.Actions[indexOfNextActionToMakeHeroic].ActionType.MakeHeroic();
					break;
				case PlayerSpecialization.SquadLeader:
					if (performingPlayer.BattleBots != null)
					{
						if (performingPlayer.BattleBots.IsDisabled)
							performingPlayer.BattleBots.IsDisabled = false;
						else
							SittingDuck.ZonesByLocation[StationLocation.ZoneLocation()].RepairFirstDamage(performingPlayer);
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
		}

		private void RestoreThreatMovement()
		{
			SittingDuck.ThreatController.EndOfTurn -= RestoreThreatMovement;
			//TODO: Bug: This won't remove the threat movement debuff, since 'this' isn't the cause, the player is.
			SittingDuck.ThreatController.RemoveExternalThreatEffectForSource(this);
		}

		private void PerformAdvancedSpecialization(Player performingPlayer, int currentTurn)
		{
			switch (performingPlayer.BasicSpecialization)
			{
				case PlayerSpecialization.DataAnalyst:
					PerformCAction(performingPlayer, currentTurn, false, StationLocation == StationLocation.LowerWhite);
					performingPlayer.BonusPoints++;
					break;
				case PlayerSpecialization.EnergyTechnician:
					if (StationLocation.IsUpperDeck())
						foreach (var zone in SittingDuck.Zones)
						{
							var isCurrentZone = (StationLocation.ZoneLocation() == zone.ZoneLocation);
							zone.UpperStation.Shield.BonusShield += isCurrentZone ? 2 : 1;
						}
					break;
				case PlayerSpecialization.Hypernavigator:
					if (currentTurn == 9 || currentTurn == 10)
						SittingDuck.Game.NumberOfTurns = currentTurn + 1;
					break;
				case PlayerSpecialization.Mechanic:
					var firstThreat = new[] {PlayerActionType.A, PlayerActionType.B, PlayerActionType.C}
						.Select(actionType => GetFirstThreatOfType(actionType, performingPlayer))
						.FirstOrDefault(threat => threat != null);
					if(firstThreat != null)
						DamageThreat(2, firstThreat, performingPlayer, false);
					break;
				case PlayerSpecialization.Medic:
					performingPlayer.PreventsKnockOut = true;
					break;
				case PlayerSpecialization.PulseGunner:
					PerformAAction(performingPlayer, false, StationLocation == StationLocation.LowerWhite);
					break;
				case PlayerSpecialization.Rocketeer:
					PerformCAction(performingPlayer, currentTurn, false, StationLocation == StationLocation.LowerBlue);
					break;
				case PlayerSpecialization.SpecialOps:
					throw new NotImplementedException();
				case PlayerSpecialization.SquadLeader:
					var canGoIntoSpace = performingPlayer.BattleBots != null &&
						!performingPlayer.BattleBots.IsDisabled &&
						StationLocation != StationLocation.LowerBlue;
					if (canGoIntoSpace)
					{
						MovementController.MoveHeroically(performingPlayer, StationLocation.UpperRed, currentTurn);
						var newStation = SittingDuck.StandardStationsByLocation[StationLocation];
						if(newStation.StationLocation == StationLocation.UpperRed && newStation.CanUseCComponent(performingPlayer))
							newStation.PerformCAction(performingPlayer, currentTurn);
					}
					break;
				case PlayerSpecialization.Teleporter:
					var newStationLocation = StationLocation.DiagonalStation();
					if (newStationLocation != null)
						performingPlayer.CurrentStation = SittingDuck.StandardStationsByLocation[newStationLocation.Value];
					break;
				default:
					throw new InvalidOperationException("Missing specialization when attempting advanced specialization.");
			}
		}
	}
}
