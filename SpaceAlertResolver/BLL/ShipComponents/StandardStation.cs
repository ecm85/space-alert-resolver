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

		private bool HasIrreparableMalfunctionOfType(PlayerAction playerAction)
		{
			return IrreparableMalfunctions.Any(malfunction => malfunction.ActionType == playerAction);
		}

		private void PerformBAction(Player performingPlayer, bool isHeroic, bool isRemote = false)
		{
			var firstBThreat = GetFirstThreatOfType(PlayerAction.B, performingPlayer);
			if (firstBThreat != null)
			{
				if (!(isRemote && firstBThreat.NextDamageWillDestroyThreat()))
					DamageThreat(isHeroic ? 2 : 1, firstBThreat, performingPlayer, isHeroic);
			}
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.B))
				RefillEnergy(isHeroic);
		}

		private PlayerDamage[] PerformAAction(Player performingPlayer, bool isHeroic, bool isAdvanced = false)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A, performingPlayer);
			if (firstAThreat != null)
			{
				DamageThreat(isHeroic ? 2 : 1, firstAThreat, performingPlayer, isHeroic);
				Cannon.RemoveMechanicBuff();
				return null;
			}
			if (HasIrreparableMalfunctionOfType(PlayerAction.A))
			{
				Cannon.RemoveMechanicBuff();
				return null;
			}
			return Cannon.PerformAAction(isHeroic, performingPlayer, isAdvanced);
		}

		private bool CanFireCannon(Player performingPlayer)
		{
			var firstAThreat = GetFirstThreatOfType(PlayerAction.A, performingPlayer);
			return firstAThreat == null && !HasIrreparableMalfunctionOfType(PlayerAction.A) && Cannon.CanFire();
		}

		private void PerformCAction(Player performingPlayer, int currentTurn, bool isRemote = false, bool isAdvanced = false)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C, performingPlayer);
			if (firstCThreat != null)
			{
				if (!(isRemote && firstCThreat.NextDamageWillDestroyThreat()))
					DamageThreat(1, firstCThreat, performingPlayer, false);
			}
			else if (!HasIrreparableMalfunctionOfType(PlayerAction.C))
				CComponent.PerformCAction(performingPlayer, currentTurn, isAdvanced);
		}

		private bool CanUseCComponent(Player performingPlayer)
		{
			var firstCThreat = GetFirstThreatOfType(PlayerAction.C, performingPlayer);
			return firstCThreat == null && !HasIrreparableMalfunctionOfType(PlayerAction.C) && CComponent.CanPerformCAction(performingPlayer);
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
					return PerformAAction(player, false);
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
					return PerformAAction(player, true);
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
					throw new NotImplementedException();
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
					throw new NotImplementedException();
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
					throw new NotImplementedException();
				case PlayerSpecialization.Mechanic:
					var firstThreat = new[] {PlayerAction.A, PlayerAction.B, PlayerAction.C}
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
					throw new NotImplementedException();
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
