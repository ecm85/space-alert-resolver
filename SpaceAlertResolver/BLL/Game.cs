using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;

namespace BLL
{
	public class Game
	{
		//TODO: Variable-range interceptors (see scattered todos)
		//TODO: Specializations
		//TODO: Red threats
		//TODO: Double actions
		//TODO: Campaign repairs and damage carryover
		//TODO: Let user select damage tokens
		//TODO: include penalties in score, and break score up more?
		//TODO: Make threat buff container a separate object instead of a list on ISittingDuck, make a ctor argument to external threats
		//TODO: Threat factory, threat enum
		//TODO: Pick perform or on for event names. Stop using both! Maybe Do?
		//TODO: Change all threat display names to include threat #
		//TODO: Add threats OnThreatTerminated method and move common calls from both On methods into it (and in all children)
		//TODO: Make damage an event
		//TODO: Figure out a way to not double-enter internal threats locations (in both the threat and the station)
		private readonly SittingDuck sittingDuck;
		private readonly IList<Player> players;
		private int nextTurn;
		public const int NumberOfTurns = 12;
		private readonly IList<int> phaseStartTurns = new[] {1, 4, 8};
		public int TotalPoints { get; private set; }
		public bool AllowVariableRangeInteceptors { get; set; }
		public ThreatController ThreatController { get; private set; }

		public Game(
			SittingDuck sittingDuck,
			IList<Player> players,
			ThreatController threatController)
		{
			this.sittingDuck = sittingDuck;
			ThreatController = threatController;
			this.players = players;
			PadPlayerActions();
			nextTurn = 0;
		}

		private void PadPlayerActions()
		{
			foreach (var player in players)
				player.Actions.AddRange(Enumerable.Repeat(PlayerAction.None, NumberOfTurns - player.Actions.Count));
		}

		public void PerformTurn()
		{
			var currentTurn = nextTurn;
			ThreatController.AddNewThreatsToTracks(currentTurn);
			PerformPlayerActionsAndResolveDamage(currentTurn);
			ThreatController.MoveThreats(currentTurn);
			PerformEndOfTurn();
			var isSecondTurnOfPhase = phaseStartTurns.Contains(currentTurn - 1);
			if (isSecondTurnOfPhase)
				CheckForComputer(currentTurn);
			var isEndOfPhase = phaseStartTurns.Contains(currentTurn + 1);
			if (isEndOfPhase)
				PerformEndOfPhase();
			if (currentTurn == NumberOfTurns - 1)
			{
				ThreatController.MoveThreats(currentTurn + 1);
				var rocketFiredLastTurn = sittingDuck.RocketsComponent.RocketFiredLastTurn;
				if (rocketFiredLastTurn != null)
					ResolveDamage(new [] {rocketFiredLastTurn.PerformAttack()}, null);
				CalculateScore();
				ThreatController.JumpToHyperspace();
			}
			nextTurn++;
		}

		private void CalculateScore()
		{
			TotalPoints += sittingDuck.VisualConfirmationComponent.TotalVisualConfirmationPoints;
			TotalPoints += ThreatController.InternalThreats.Sum(threat => threat.Points);
			TotalPoints += ThreatController.ExternalThreats.Sum(threat => threat.Points);
		}

		private void PerformEndOfPhase()
		{
			sittingDuck.VisualConfirmationComponent.PerformEndOfPhase();
			sittingDuck.Computer.PerformEndOfPhase();
		}

		private void CheckForComputer(int currentTurn)
		{
			if (!sittingDuck.Computer.MaintenancePerformedThisPhase)
				foreach (var player in players)
					player.Shift(currentTurn + 1);
		}

		private void PerformPlayerActionsAndResolveDamage(int currentTurn)
		{
			var damages = new List<PlayerDamage>();
			foreach (var player in players.Where(player => !player.IsKnockedOut))
			{
				var playerAction = player.Actions[currentTurn];
				PerformPlayerAction(currentTurn, playerAction, player, damages);
			}
			ThreatController.PerformEndOfPlayerActions();

			var rocketFiredLastTurn = sittingDuck.RocketsComponent.RocketFiredLastTurn;
			if (rocketFiredLastTurn != null)
				damages.Add(rocketFiredLastTurn.PerformAttack());
			var interceptorDamages = sittingDuck.InterceptorStation.PlayerInterceptorDamage;
			ResolveDamage(damages, interceptorDamages);
		}

		private void PerformPlayerAction(int currentTurn, PlayerAction playerAction, Player player, List<PlayerDamage> damages)
		{
			switch (playerAction)
			{
				case PlayerAction.A:
					var damage = player.CurrentStation.PerformAAction(player, currentTurn, false);
					if (damage != null)
						damages.Add(damage);
					break;
				case PlayerAction.B:
					player.CurrentStation.PerformBAction(player, currentTurn, false);
					break;
				case PlayerAction.C:
					player.CurrentStation.PerformCAction(player, currentTurn);
					break;
				case PlayerAction.MoveBlue:
					MovePlayer(player.CurrentStation.BluewardStation, player);
					break;
				case PlayerAction.MoveRed:
					MovePlayer(player.CurrentStation.RedwardStation, player);
					break;
				case PlayerAction.ChangeDeck:
					var currentZone = sittingDuck.ZonesByLocation[player.CurrentStation.StationLocation.ZoneLocation()];
					MovePlayer(player.CurrentStation.OppositeDeckStation, player);
					if (currentZone.Gravolift.ShiftsPlayers)
						player.Shift(currentTurn + 1);
					currentZone.Gravolift.SetOccupied();
					break;
				case PlayerAction.BattleBots:
					if (!player.BattleBots.IsDisabled)
						player.CurrentStation.UseBattleBots(player, false);
					break;
				case PlayerAction.None:
					player.CurrentStation.PerformNoAction(player);
					break;
				case PlayerAction.HeroicA:
					player.CurrentStation.PerformAAction(player, currentTurn, true);
					break;
				case PlayerAction.HeroicB:
					player.CurrentStation.PerformBAction(player, currentTurn, true);
					break;
				case PlayerAction.HeroicBattleBots:
					player.CurrentStation.UseBattleBots(player, true);
					break;
				case PlayerAction.TeleportBlueLower:
					MovePlayer(sittingDuck.BlueZone.LowerStation, player);
					break;
				case PlayerAction.TeleportBlueUpper:
					MovePlayer(sittingDuck.BlueZone.UpperStation, player);
					break;
				case PlayerAction.TeleportWhiteLower:
					MovePlayer(sittingDuck.WhiteZone.LowerStation, player);
					break;
				case PlayerAction.TeleportWhiteUpper:
					MovePlayer(sittingDuck.WhiteZone.UpperStation, player);
					break;
				case PlayerAction.TeleportRedLower:
					MovePlayer(sittingDuck.RedZone.LowerStation, player);
					break;
				case PlayerAction.TeleportRedUpper:
					MovePlayer(sittingDuck.RedZone.UpperStation, player);
					break;
			}
		}

		private void PerformEndOfTurn()
		{
			foreach (var zone in sittingDuck.Zones)
			{
				zone.Gravolift.PerformEndOfTurn();
				zone.UpperStation.Cannon.PerformEndOfTurn();
				zone.LowerStation.Cannon.PerformEndOfTurn();
			}
			sittingDuck.VisualConfirmationComponent.PerformEndOfTurn();
			sittingDuck.RocketsComponent.PerformEndOfTurn();
			sittingDuck.InterceptorStation.PerformEndOfTurn();
			ThreatController.PerformEndOfTurn();
		}

		private static void MovePlayer(Station newDestination, Player player)
		{
			var newStation = newDestination ?? player.CurrentStation;
			var oldStation = player.CurrentStation;
			player.CurrentStation = newStation;
			oldStation.Players.Remove(player);
			newStation.Players.Add(player);
		}

		private void ResolveDamage(IEnumerable<PlayerDamage> damages, PlayerInterceptorDamage interceptorDamages)
		{
			if (!ThreatController.DamageableExternalThreats.Any())
				return;
			var damagesByThreat = new Dictionary<ExternalThreat, IList<PlayerDamage>>();
			foreach (var damage in damages)
			{
				var priorityThreatsInRange = ThreatController.DamageableExternalThreats.Where(threat => threat.IsPriorityTargetFor(damage) && threat.CanBeTargetedBy(damage)).ToList();
				var threatsInRange = ThreatController.DamageableExternalThreats.Where(threat => threat.CanBeTargetedBy(damage)).ToList();
				switch (damage.PlayerDamageType.DamageTargetType())
				{
					case DamageTargetType.All:
						foreach (var threat in threatsInRange)
							AddToDamagesByThreat(threat, damage, damagesByThreat);
						break;
					case DamageTargetType.Single:
						var priorityThreatHit = priorityThreatsInRange.OrderBy(threat => threat.TimeAppears).FirstOrDefault();
						if (priorityThreatHit != null)
							AddToDamagesByThreat(priorityThreatHit, damage, damagesByThreat);
						else
						{
							var threatHit = threatsInRange.OrderBy(threat => threat.Position).ThenBy(threat => threat.TimeAppears).FirstOrDefault();
							if (threatHit != null)
								AddToDamagesByThreat(threatHit, damage, damagesByThreat);
						}
						break;
					default:
						throw new InvalidOperationException();
				}
			}
			if (interceptorDamages != null)
				AddInterceptorDamages(interceptorDamages, damagesByThreat);

			foreach (var threat in damagesByThreat.Keys)
				threat.TakeDamage(damagesByThreat[threat]);

			ThreatController.PerformEndOfDamageResolution();
		}

		private void AddInterceptorDamages(PlayerInterceptorDamage interceptorDamages, Dictionary<ExternalThreat, IList<PlayerDamage>> damagesByThreat)
		{
			var interceptorDamagesMultiple = interceptorDamages.MultipleDamage;
			var threatsHitByInterceptors = ThreatController.ExternalThreats.Where(threat => threat.CanBeTargetedBy(interceptorDamagesMultiple)).ToList();
			if (threatsHitByInterceptors.Count() > 1)
				foreach (var threat in threatsHitByInterceptors)
					AddToDamagesByThreat(threat, interceptorDamagesMultiple, damagesByThreat);
			else if (threatsHitByInterceptors.Count() == 1)
				AddToDamagesByThreat(threatsHitByInterceptors.Single(), interceptorDamages.SingleDamage, damagesByThreat);
		}

		private static void AddToDamagesByThreat(
			ExternalThreat threat,
			PlayerDamage damage,
			IDictionary<ExternalThreat, IList<PlayerDamage>> damagesByThreat)
		{
			if(!damagesByThreat.ContainsKey(threat) || damagesByThreat[threat] == null)
				damagesByThreat[threat] = new List<PlayerDamage>();
			damagesByThreat[threat].Add(damage);
		}
	}
}
