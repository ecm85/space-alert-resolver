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
		//TODO: Feature: Double actions
		//TODO: Feature: Campaign repairs and damage carryover
		//TODO: Feature: Let user select damage tokens
		//TODO: Feature: include penalties in score, and break score up more?
		//TODO: Code Cleanup: Threat factory, threat enum
		//TODO: Code Cleanup: Pick perform or on for event names. Stop using both! Maybe Do?
		//TODO: Feature: Change all threat display names to include threat #
		//TODO: Code Cleanup: Make damage an event
		//TODO: Rules clarification: Does a person heroically moving occupy the lift?
		//TODO: Code Cleanup: Make reusable threat 'components'
		//TODO: Code Cleanup: Change energy from int to actual blocks?
		//TODO: Code Cleanup: Change all the PlayerDamage[] to IList<PlayerDamage> or IEnumerable<PlayerDamage> because what was i thinking.
		//TODO: Unit test pulse cannon and laser cannon
		//TODO: Code Cleanup: Change all mechanic buff removals to be event-based, and always fire 'tried to use cannon' event
		//TODO: Code Cleanup: Revisit construction and threatcontroller -> game -> sittingduck -> threats dependency graph
		//TODO: Double actions and Specializations: Change move-out to only fire before an 'turn' that has a movement and move-in to only fire after
		//TODO: Change guns to only fire at the end of turn, and make aciton methods stop returning things, ala interceptors
		public SittingDuck SittingDuck { get; private set; }
		private readonly IList<Player> players;
		private int nextTurn;
		public int NumberOfTurns = 12;
		private readonly IList<int> phaseStartTurns = new[] {1, 4, 8};
		public int TotalPoints { get; private set; }
		public ThreatController ThreatController { get; private set; }

		public Game(
			IList<Player> players,
			ThreatController threatController)
		{
			SittingDuck = new SittingDuck(threatController, this);
			ThreatController = threatController;
			this.players = players;
			PadPlayerActions();
			SetCaptain();
			nextTurn = 0;
		}

		private void SetCaptain()
		{
			players[0].IsCaptain = true;
			foreach (var player in players.Except(new[] {players[0]}))
				player.IsCaptain = false;
		}

		private void PadPlayerActions()
		{
			foreach (var player in players)
				player.Actions.AddRange(Enumerable.Repeat((PlayerAction?)null, NumberOfTurns - player.Actions.Count));
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
				var rocketFiredLastTurn = SittingDuck.RocketsComponent.RocketFiredLastTurn;
				if (rocketFiredLastTurn != null)
					ResolveDamage(new [] {rocketFiredLastTurn.PerformAttack(null)}, null);
				foreach(var player in SittingDuck.InterceptorStations.Where(station => station.StationLocation.DistanceFromShip() > 1).SelectMany(station => station.Players))
				{
					player.IsKnockedOut = true;
					player.BattleBots.IsDisabled = true;
				}
				CalculateScore();
				ThreatController.JumpToHyperspace();
			}
			nextTurn++;
		}

		private void CalculateScore()
		{
			TotalPoints += SittingDuck.VisualConfirmationComponent.TotalVisualConfirmationPoints;
			TotalPoints += ThreatController.TotalThreatPoints;
			TotalPoints += players.Sum(player => player.BonusPoints);
		}

		private void PerformEndOfPhase()
		{
			SittingDuck.VisualConfirmationComponent.PerformEndOfPhase();
			SittingDuck.Computer.PerformEndOfPhase();
		}

		private void CheckForComputer(int currentTurn)
		{
			if (!SittingDuck.Computer.MaintenancePerformedThisPhase)
				foreach (var player in players)
					player.Shift(currentTurn + 1);
		}

		private void PerformPlayerActionsAndResolveDamage(int currentTurn)
		{
			var damages = players
				.Where(player => !player.IsKnockedOut)
				.Select(player => player.CurrentStation.PerformPlayerAction(player, currentTurn))
				.Where(damageList => damageList != null)
				.SelectMany(damageList => damageList)
				.ToList();
			ThreatController.PerformEndOfPlayerActions();

			var rocketFiredLastTurn = SittingDuck.RocketsComponent.RocketFiredLastTurn;
			if (rocketFiredLastTurn != null)
				damages.Add(rocketFiredLastTurn.PerformAttack(null));
			var interceptorDamages = SittingDuck.InterceptorStations
				.Select(station => station.PlayerInterceptorDamage)
				.Where(damage => damage != null);
			var anyPlayersInInterceptors = SittingDuck.InterceptorStations.SelectMany(station => station.Players).Any();
			var visualConfirmationPerformed = SittingDuck.VisualConfirmationComponent.NumberOfConfirmationsThisTurn > 0;
			var targetingAssistanceProvided = anyPlayersInInterceptors || visualConfirmationPerformed;
			if (!targetingAssistanceProvided)
				damages = damages.Where(damage => !damage.RequiresTargetingAssistance).ToList();
			ResolveDamage(damages, interceptorDamages);
		}

		private void PerformEndOfTurn()
		{
			foreach (var zone in SittingDuck.Zones)
			{
				zone.Gravolift.PerformEndOfTurn();
				zone.UpperStation.PerformEndOfTurn();
				zone.LowerStation.PerformEndOfTurn();
			}
			SittingDuck.VisualConfirmationComponent.PerformEndOfTurn();
			SittingDuck.RocketsComponent.PerformEndOfTurn();
			foreach (var interceptorStation in SittingDuck.InterceptorStations)
				interceptorStation.PerformEndOfTurn();
			ThreatController.PerformEndOfTurn();
		}

		private void ResolveDamage(IEnumerable<PlayerDamage> damages, IEnumerable<PlayerInterceptorDamage> interceptorDamages)
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
			foreach(var interceptorDamage in interceptorDamages)
				AddInterceptorDamages(interceptorDamage, damagesByThreat);

			foreach (var threat in damagesByThreat.Keys)
				threat.TakeDamage(damagesByThreat[threat]);

			ThreatController.PerformEndOfDamageResolution();
		}

		private void AddInterceptorDamages(PlayerInterceptorDamage interceptorDamages, Dictionary<ExternalThreat, IList<PlayerDamage>> damagesByThreat)
		{
			var interceptorDamagesMultiple = interceptorDamages.MultipleDamage;
			var threatsHitByInterceptors = ThreatController.DamageableExternalThreats.Where(threat => threat.CanBeTargetedBy(interceptorDamagesMultiple)).ToList();
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
