using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class StandardStation : Station
	{
		private Cannon Cannon { get; set; }
		private Gravolift Gravolift { get; set; }
		private Airlock BluewardAirlock { get; set; }
		private Airlock RedwardAirlock { get; set; }
		private SittingDuck SittingDuck { get; set; }
		private IBravoComponent BravoComponent { get; set; }
		private ICharlieComponent CharlieComponent { get; set; }

		protected StandardStation(
			StationLocation stationLocation,
			ThreatController threatController,
			IBravoComponent bravoComponent,
			ICharlieComponent charlieComponent,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			Cannon cannon,
			SittingDuck sittingDuck) : base(stationLocation, threatController)
		{
			BravoComponent = bravoComponent;
			CharlieComponent = charlieComponent;
			Gravolift = gravolift;
			BluewardAirlock = bluewardAirlock;
			RedwardAirlock = redwardAirlock;
			Cannon = cannon;
			SittingDuck = sittingDuck;
		}

		public void SetOpticsDisrupted(bool opticsDisrupted)
		{
			Cannon.SetOpticsDisrupted(opticsDisrupted);
		}

		public abstract void DrainEnergy(int amount);

		public IDamageableComponent GetDamageableBravoComponent()
		{
			return BravoComponent;
		}

		public IDamageableComponent GetDamageableAlphaComponent()
		{
			return Cannon;
		}

		public virtual void PerformEndOfTurn()
		{
			Cannon.PerformEndOfTurn();
		}

		private bool HasIrreparableMalfunctionOfType(PlayerActionType playerActionType)
		{
			return IrreparableMalfunctions.Any(malfunction => malfunction.ActionType == playerActionType);
		}

		private bool CanFireCannon(Player performingPlayer)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerActionType.A, performingPlayer);
			return firstAThreat == null && !HasIrreparableMalfunctionOfType(PlayerActionType.A) && Cannon.CanFire();
		}

		private bool CanUseCharlieComponent(Player performingPlayer)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerActionType.C, performingPlayer);
			return firstCThreat == null && !HasIrreparableMalfunctionOfType(PlayerActionType.C) && CharlieComponent.CanPerformCAction(performingPlayer);
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

		private void PerformAAction(Player performingPlayer, bool isHeroic, bool isAdvanced = false)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerActionType.A, performingPlayer);
			if (firstAThreat != null)
				DamageThreat(isHeroic ? 2 : 1, firstAThreat, performingPlayer, isHeroic);
			else if (!HasIrreparableMalfunctionOfType(PlayerActionType.A))
				Cannon.PerformAAction(isHeroic, performingPlayer, isAdvanced);
			Cannon.RemoveMechanicBuff();
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
				BravoComponent.PerformBAction(isHeroic);
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

			if (performingPlayer.IsPerformingBasicMedic(currentTurn))
				PerformBasicSpecialization(performingPlayer, currentTurn);
			if (performingPlayer.IsPerformingAdvancedMedic(currentTurn))
				PerformAdvancedSpecialization(performingPlayer, currentTurn);
			if (performingPlayer.IsPerformingAdvancedSpecialOps(currentTurn))
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
						SittingDuck.ThreatController.AddExternalThreatEffect(ExternalThreatEffect.ReducedMovement, ThreatController.SingleTurnThreatSource);
					break;
				case PlayerSpecialization.Mechanic:
					Cannon.AddMechanicBuff();
					break;
				case PlayerSpecialization.Medic:
					var actionsToMakeHeroic = Players
						.Except(new[] {performingPlayer})
						.Select(player => player.Actions[currentTurn])
						.Where(action => action.CanBeMadeHeroic());
					foreach (var action in actionsToMakeHeroic)
						action.MakeHeroic();
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
					var indexOfNextActionToMakeHeroic = performingPlayer.Actions.ToList().FindIndex(currentTurn + 1, action => action.CanBeMadeHeroic());
					performingPlayer.Actions[indexOfNextActionToMakeHeroic].MakeHeroic();
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
							zone.UpperStation.AddBonusShield(isCurrentZone ? 2 : 1);
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
					performingPlayer.SetPreventsKnockOut(true);
					break;
				case PlayerSpecialization.PulseGunner:
					PerformAAction(performingPlayer, false, StationLocation == StationLocation.LowerWhite);
					break;
				case PlayerSpecialization.Rocketeer:
					PerformCAction(performingPlayer, currentTurn, false, StationLocation == StationLocation.LowerBlue);
					break;
				case PlayerSpecialization.SpecialOps:
					//This spec does nothing on its actual turn, and only needs to remove the protection granted
					performingPlayer.HasSpecialOpsProtection = false;
					break;
				case PlayerSpecialization.SquadLeader:
					var canGoIntoSpace = performingPlayer.BattleBots != null &&
						!performingPlayer.BattleBots.IsDisabled &&
						StationLocation != StationLocation.LowerBlue;
					if (canGoIntoSpace)
					{
						MovementController.MoveHeroically(SittingDuck.StandardStationsByLocation, performingPlayer, StationLocation.UpperRed, currentTurn);
						var newStation = SittingDuck.StandardStationsByLocation[StationLocation];
						if(newStation.StationLocation == StationLocation.UpperRed && newStation.CanUseCharlieComponent(performingPlayer))
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

		public IEnumerable<PlayerDamage> CurrentPlayerDamage()
		{
			return Cannon.CurrentPlayerDamage;
		}
	}
}
