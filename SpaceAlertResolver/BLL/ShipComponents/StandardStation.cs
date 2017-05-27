using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.Threats;

namespace BLL.ShipComponents
{
	public abstract class StandardStation : Station
	{
		public abstract IAlphaComponent AlphaComponent { get; }
		public abstract IBravoComponent BravoComponent { get; }
		protected abstract ICharlieComponent CharlieComponent { get; }

		private Gravolift Gravolift { get; }
		private Doors BluewardDoors { get; }
		private Doors RedwardDoors { get; }
		private SittingDuck SittingDuck { get; }

		protected StandardStation(
			StationLocation stationLocation,
			ThreatController threatController,
			Gravolift gravolift,
			Doors bluewardDoors,
			Doors redwardDoors,
			SittingDuck sittingDuck) : base(stationLocation, threatController)
		{
			Gravolift = gravolift;
			BluewardDoors = bluewardDoors;
			RedwardDoors = redwardDoors;
			SittingDuck = sittingDuck;
		}

		public void SetOpticsDisrupted(bool opticsDisrupted)
		{
			AlphaComponent.SetOpticsDisrupted(opticsDisrupted);
		}

		public abstract void DrainEnergy(int amount);

		public IDamageableComponent DamageableBravoComponent => BravoComponent;

		public IDamageableComponent DamageableAlphaComponent => AlphaComponent;

		public virtual void PerformEndOfTurn()
		{
			AlphaComponent.PerformEndOfTurn();
		}

		private bool HasIrreparableMalfunctionOfType(PlayerActionType playerActionType)
		{
			return IrreparableMalfunctions.Any(malfunction => malfunction.ActionType == playerActionType);
		}

		private bool CanFireCannon(Player performingPlayer)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerActionType.Alpha, performingPlayer);
			return firstAThreat == null && !HasIrreparableMalfunctionOfType(PlayerActionType.Alpha) && AlphaComponent.CanFire();
		}

		private bool CanUseCharlieComponent(Player performingPlayer)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerActionType.Charlie, performingPlayer);
			return firstCThreat == null && !HasIrreparableMalfunctionOfType(PlayerActionType.Charlie) && CharlieComponent.CanPerformCAction(performingPlayer);
		}

		public bool PerformMoveOutTowardsRed(Player performingPlayer, int currentTurn)
		{
			if (!CanMoveOutTowardsRed())
				return false;
			OnPlayerMovingOut(performingPlayer, currentTurn);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public bool PerformMoveOutTowardsOppositeDeck(Player performingPlayer, int currentTurn, bool isHeroic)
		{
			OnPlayerMovingOut(performingPlayer, currentTurn);
			Gravolift.Use(performingPlayer, currentTurn, isHeroic);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public bool PerformMoveOutTowardsBlue(Player performingPlayer, int currentTurn)
		{
			if (!CanMoveOutTowardsBlue())
				return false;
			OnPlayerMovingOut(performingPlayer, currentTurn);
			Players.Remove(performingPlayer);
			performingPlayer.CurrentStation = null;
			return true;
		}

		public override void MovePlayerIn(Player performingPlayer, int? currentTurn = null)
		{
			OnPlayerMovingIn(performingPlayer, currentTurn);
			Players.Add(performingPlayer);
			performingPlayer.CurrentStation = this;
		}

		public bool CanMoveOutTowardsRed()
		{
			return RedwardDoors != null && RedwardDoors.CanUse;
		}

		public static bool CanMoveOutTowardsOppositeDeck()
		{
			return true;
		}

		public bool CanMoveOutTowardsBlue()
		{
			return BluewardDoors != null && BluewardDoors.CanUse;
		}

		private void PerformAAction(Player performingPlayer, bool isHeroic, bool isAdvanced = false)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerActionType.Alpha, performingPlayer);
			if (firstAThreat != null)
				DamageThreat(isHeroic ? 2 : 1, firstAThreat, performingPlayer, isHeroic);
			else if (!HasIrreparableMalfunctionOfType(PlayerActionType.Alpha))
				AlphaComponent.PerformAAction(isHeroic, performingPlayer, isAdvanced);
			AlphaComponent.RemoveMechanicBuff();
		}

		private void PerformBAction(Player performingPlayer, bool isHeroic, bool isRemote = false)
		{
			var firstBThreat = GetFirstThreatOfType(PlayerActionType.Bravo, performingPlayer);
			if (firstBThreat != null)
			{
				if (!(isRemote && firstBThreat.NextDamageWillDestroyThreat()))
					DamageThreat(isHeroic ? 2 : 1, firstBThreat, performingPlayer, isHeroic);
			}
			else if (!HasIrreparableMalfunctionOfType(PlayerActionType.Bravo))
				BravoComponent.PerformBAction(isHeroic);
		}

		private void PerformCAction(Player performingPlayer, int currentTurn, bool isRemote = false, bool isAdvanced = false)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerActionType.Charlie, performingPlayer);
			if (firstCThreat != null)
			{
				if (!(isRemote && firstCThreat.NextDamageWillDestroyThreat()))
					DamageThreat(1, firstCThreat, performingPlayer, false);
			}
			else if (!HasIrreparableMalfunctionOfType(PlayerActionType.Charlie) && CharlieComponent.CanPerformCAction(performingPlayer))
				CharlieComponent.PerformCAction(performingPlayer, currentTurn, isAdvanced);
		}

		private void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			if (performingPlayer.BattleBots == null || performingPlayer.BattleBots.IsDisabled)
				return;
			var firstBattleBotThreat = GetFirstThreatOfType(PlayerActionType.BattleBots, performingPlayer);
			if (firstBattleBotThreat != null)
				DamageThreat(1, firstBattleBotThreat, performingPlayer, isHeroic);
		}

		public override void PerformNextPlayerAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.MarkNextActionPerforming(currentTurn);
			var actionType = performingPlayer.GetNextActionToPerform(currentTurn);
			switch (actionType)
			{
				case PlayerActionType.Alpha:
					PerformAAction(performingPlayer, false);
					break;
				case PlayerActionType.Bravo:
					PerformBAction(performingPlayer, false);
					break;
				case PlayerActionType.Charlie:
					PerformCAction(performingPlayer, currentTurn);
					break;
				case PlayerActionType.MoveBlue:
					MovementController.MoveBlue(SittingDuck.StandardStationsByLocation, performingPlayer, currentTurn);
					break;
				case PlayerActionType.MoveRed:
					MovementController.MoveRed(SittingDuck.StandardStationsByLocation, performingPlayer, currentTurn);
					break;
				case PlayerActionType.ChangeDeck:
					MovementController.ChangeDeck(SittingDuck.StandardStationsByLocation, performingPlayer, currentTurn);
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
					MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.LowerBlue, currentTurn);
					break;
				case PlayerActionType.TeleportBlueUpper:
					MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.UpperBlue, currentTurn);
					break;
				case PlayerActionType.TeleportWhiteLower:
					MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.LowerWhite, currentTurn);
					break;
				case PlayerActionType.TeleportWhiteUpper:
					MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.UpperWhite, currentTurn);
					break;
				case PlayerActionType.TeleportRedLower:
					MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.LowerRed, currentTurn);
					break;
				case PlayerActionType.TeleportRedUpper:
					MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.UpperRed, currentTurn);
					break;
				case PlayerActionType.BasicSpecialization:
					PerformBasicSpecialization(performingPlayer, currentTurn);
					break;
				case PlayerActionType.AdvancedSpecialization:
					PerformAdvancedSpecialization(performingPlayer, currentTurn);
					break;
			}
			performingPlayer.MarkNextActionPerformed(currentTurn);
		}

		private void PerformBasicSpecialization(Player performingPlayer, int currentTurn)
		{
			switch (performingPlayer.Specialization)
			{
				case PlayerSpecialization.DataAnalyst:
					SittingDuck.StandardStationsByLocation[StationLocation.UpperWhite].PerformCAction(performingPlayer, currentTurn, true);
					performingPlayer.BonusPoints++;
					break;
				case PlayerSpecialization.EnergyTechnician:
					SittingDuck.StandardStationsByLocation[StationLocation.LowerWhite].PerformBAction(performingPlayer, false, true);
					break;
				case PlayerSpecialization.Hypernavigator:
					BasicHypernavigator();
					break;
				case PlayerSpecialization.Mechanic:
					AlphaComponent.AddMechanicBuff();
					break;
				case PlayerSpecialization.Medic:
					PerformBasicMedic(performingPlayer, currentTurn);
					break;
				case PlayerSpecialization.PulseGunner:
					PerformBasicPulseGunner(performingPlayer);
					break;
				case PlayerSpecialization.Rocketeer:
					SittingDuck.StandardStationsByLocation[StationLocation.LowerBlue].PerformCAction(performingPlayer, currentTurn, true);
					break;
				case PlayerSpecialization.SpecialOps:
					var actionToMakeHeroic = performingPlayer.Actions.Skip(currentTurn).FirstOrDefault(action => action.CanBeMadeHeroic());
					actionToMakeHeroic?.MakeHeroic();
					break;
				case PlayerSpecialization.SquadLeader:
					PerformBasicSquadLeader(performingPlayer);
					break;
				case PlayerSpecialization.Teleporter:
					var playerToTeleport = SittingDuck.PlayersOnShip.SingleOrDefault(playerOnShip => playerOnShip.PlayerToTeleport);
					var teleportDestination = SittingDuck.PlayersOnShip.SingleOrDefault(playerOnShip => playerOnShip.TeleportDestination);
					if(playerToTeleport != null && teleportDestination != null)
						playerToTeleport.CurrentStation = teleportDestination.CurrentStation;
					break;
				default:
					throw new InvalidOperationException("Missing specialization when attempting basic specialization.");
			}
		}

		private void BasicHypernavigator()
		{
			if (StationLocation.IsLowerDeck())
				SittingDuck.ThreatController.AddSingleTurnExternalThreatEffect(ThreatStatus.ReducedMovement);
		}

		private void PerformBasicMedic(Player performingPlayer, int currentTurn)
		{
			var actionsToMakeHeroic = Players
				.Except(new[] {performingPlayer})
				.Select(player => player.GetActionForTurn(currentTurn))
				.Where(action => action.CanBeMadeHeroic());
			foreach (var action in actionsToMakeHeroic)
				action.MakeHeroic();
		}

		private void PerformBasicSquadLeader(Player performingPlayer)
		{
			if (performingPlayer.BattleBots == null) return;
			if (performingPlayer.BattleBots.IsDisabled)
				performingPlayer.BattleBots.IsDisabled = false;
			else
				SittingDuck.ZonesByLocation[StationLocation.ZoneLocation()].RepairFirstDamage(performingPlayer);
		}

		private void PerformBasicPulseGunner(Player performingPlayer)
		{
			var pulseCannonStation = SittingDuck.StandardStationsByLocation[StationLocation.LowerWhite];
			if (CanFireCannon(performingPlayer) && pulseCannonStation.CanFireCannon(performingPlayer) &&
			    StationLocation != StationLocation.LowerWhite)
			{
				PerformAAction(performingPlayer, false);
				pulseCannonStation.PerformAAction(performingPlayer, false);
			}
			else if (StationLocation == StationLocation.LowerWhite)
				PerformAAction(performingPlayer, false);
			else
			{
				pulseCannonStation.AlphaComponent.RemoveMechanicBuff();
				if (!CanFireCannon(performingPlayer))
					PerformAAction(performingPlayer, false);
				else
					AlphaComponent.RemoveMechanicBuff();
			}
		}

		private void PerformAdvancedSpecialization(Player performingPlayer, int currentTurn)
		{
			switch (performingPlayer.Specialization)
			{
				case PlayerSpecialization.DataAnalyst:
					PerformCAction(performingPlayer, currentTurn, false, StationLocation == StationLocation.LowerWhite);
					break;
				case PlayerSpecialization.EnergyTechnician:
					PerformAdvancedEnergyTechnician();
					break;
				case PlayerSpecialization.Hypernavigator:
					if (currentTurn == 9 || currentTurn == 10)
						SittingDuck.Game.NumberOfTurns = currentTurn + 1;
					break;
				case PlayerSpecialization.Mechanic:
					PerformAdvancedMechanic(performingPlayer);
					break;
				case PlayerSpecialization.Medic:
					performingPlayer.SetPreventsKnockOut(true);
					break;
				case PlayerSpecialization.PulseGunner:
					PerformAAction(performingPlayer, false, StationLocation == StationLocation.LowerWhite);
					break;
				case PlayerSpecialization.Rocketeer:
					PerformCAction(performingPlayer, currentTurn, false, StationLocation == StationLocation.LowerBlue);
					break;
				case PlayerSpecialization.SpecialOps:
					//This spec does nothing when playing the card - determining the protection happens earlier, and the protection lasts until end of this players action
					break;
				case PlayerSpecialization.SquadLeader:
					PerformAdvancedSquadLeader(performingPlayer, currentTurn);
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

		private void PerformAdvancedEnergyTechnician()
		{
			if (!StationLocation.IsUpperDeck()) return;
			foreach (var zone in SittingDuck.Zones)
			{
				var isCurrentZone = (StationLocation.ZoneLocation() == zone.ZoneLocation);
				zone.UpperStation.AddBonusShield(isCurrentZone ? 2 : 1);
			}
		}

		private void PerformAdvancedMechanic(Player performingPlayer)
		{
			var firstThreat = new[] {PlayerActionType.Alpha, PlayerActionType.Bravo, PlayerActionType.Charlie}
				.Select(actionType => GetFirstThreatOfType(actionType, performingPlayer))
				.FirstOrDefault(threat => threat != null);
			if (firstThreat != null)
				DamageThreat(2, firstThreat, performingPlayer, false);
		}

		private void PerformAdvancedSquadLeader(Player performingPlayer, int currentTurn)
		{
			var canGoIntoSpace = performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled && StationLocation != StationLocation.LowerBlue;
			if (!canGoIntoSpace) return;
			MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.UpperRed, currentTurn);
			var newStation = performingPlayer.CurrentStation as StandardStation;
			if (newStation != null && newStation.StationLocation == StationLocation.UpperRed && newStation.CanUseCharlieComponent(performingPlayer))
				newStation.PerformCAction(performingPlayer, currentTurn);
		}

		public IEnumerable<PlayerDamage> CurrentPlayerDamage()
		{
			return AlphaComponent.CurrentPlayerDamage;
		}
	}
}
