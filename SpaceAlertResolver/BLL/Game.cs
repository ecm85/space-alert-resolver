using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Threats;
using BLL.Tracks;

namespace BLL
{
	public class Game
	{
		private static int[] phaseStartTurns = new[] { 0, 3, 7, 12 };
		//TODO: Review the CA suppressions and the ones turned off
		//TODO: Add more functional tests
		//TODO: Feature: Double actions
		//TODO: Feature: Campaign repairs and damage carryover
		//TODO: Feature: Let user select damage tokens
		//TODO: Feature: include penalties in score, and break score up more?
		//TODO: Code Cleanup: Threat factory, threat enum
		//TODO: Feature: Change all threat display names to include threat #
		//TODO: Code Cleanup: Make damage an event
		//TODO: Rules clarification: Does a person heroically moving occupy the lift?
		//TODO: Code Cleanup: Make reusable threat 'components', Extract more common behavior to base and/or impl - such as shield killed by pulse, retaliate against battle bots, etc
		//TODO: Code Cleanup: Change energy from int to actual blocks?
		//TODO: Code Cleanup: Change all the PlayerDamage[] to IList<PlayerDamage> or IEnumerable<PlayerDamage> because what was i thinking.
		//TODO: Unit test pulse cannon and laser cannon
		//TODO: Unit test playeractionfactory
		//TODO: Code Cleanup: Change all mechanic buff removals to be event-based, and always fire 'tried to use cannon' event
		//TODO: Code Cleanup: Revisit construction and threatcontroller -> game -> sittingduck -> threats dependency graph
		//TODO: Double actions and Specializations: Change move-out to only fire before an 'turn' that has a movement and move-in to only fire after
		//TODO: Bug: Make sure all places that set a players station set it in that station too.
		//TODO: Make sure that all knocked out also disables battlebots if medic prevents knockout (and make sure spec ops behaves around parasite correctly)
		//TODO: Advanced Spec ops (can't be delayed, respect HasSpecialOpsProtection on that turn)
		//TODO: Code cleanup: Remove threat controller from all implementations of threats - make methods on Threat that subscribe to everything they care about
		public SittingDuck SittingDuck { get; }
		public IList<Player> Players { get; }
		public int CurrentTurn { get; private set; }
		public int NumberOfTurns { get; set; }
		public int TotalPoints { get; private set; }
		public ThreatController ThreatController { get; }
		public GameStatus GameStatus { get; private set; }
		public string KilledBy { get; set; }

		public event EventHandler<PhaseEventArgs> PhaseStarting = (sender, args) => { };
		public event EventHandler<PhaseEventArgs> PhaseEnded = (sender, args) => { };
		
		public Game(
			IList<Player> players,
			IList<InternalThreat> internalThreats,
			IList<ExternalThreat> externalThreats,
			IList<Threat> bonusThreats,
			IDictionary<ZoneLocation, TrackConfiguration> externalTrackConfigurationsByZone,
			TrackConfiguration internalTrackConfiguration)
		{
			GameStatus = GameStatus.InProgress;
			NumberOfTurns = 12;
			var externalTracksByZone = externalTrackConfigurationsByZone.ToDictionary(
				trackConfigurationWithZone => trackConfigurationWithZone.Key,
				trackConfigurationWithZone => new Track(trackConfigurationWithZone.Value));
			var internalTrack = new Track(internalTrackConfiguration);
			ThreatController = new ThreatController(externalTracksByZone, internalTrack, externalThreats, internalThreats);
			SittingDuck = new SittingDuck(ThreatController, this);
			var allThreats = bonusThreats.Concat(internalThreats).Concat(externalThreats);
			foreach (var threat in allThreats)
				threat.Initialize(SittingDuck, ThreatController);
			SittingDuck.SetPlayers(players);
			Players = players;
			PadPlayerActions();
		}

		public void StartGame()
		{
			PhaseStarting(this, new PhaseEventArgs {Phase = ResolutionPhase.StartGame});
			CurrentTurn = 0;
			PhaseEnded(this, new PhaseEventArgs {Phase = ResolutionPhase.StartGame});
		}

		private void PadPlayerActions()
		{
			foreach (var player in Players)
			{
				var extraNullActions = Enumerable.Repeat(PlayerActionFactory.CreateEmptyAction(), NumberOfTurns - player.Actions.Count);
				player.Actions.AddRange(extraNullActions);
			}
		}

		public void PerformTurn()
		{
			try
			{
				PhaseStarting(this, new PhaseEventArgs {Phase = ResolutionPhase.AddNewThreats});
				ThreatController.AddNewThreatsToTracks(CurrentTurn);
				PhaseEnded(this, new PhaseEventArgs { Phase = ResolutionPhase.AddNewThreats });

				CheckForAdvancedSpecialOpsProtection();

				PhaseStarting(this, new PhaseEventArgs { Phase = ResolutionPhase.PerformPlayerActions });
				PerformPlayerActions();
				PhaseEnded(this, new PhaseEventArgs { Phase = ResolutionPhase.PerformPlayerActions });

				var damage = GetStandardDamage();
				var interceptorDamage = GetInterceptorDamage();

				PhaseStarting(this, new PhaseEventArgs { Phase = ResolutionPhase.ResolveDamage });
				ResolveDamage(damage, interceptorDamage);
				PhaseEnded(this, new PhaseEventArgs { Phase = ResolutionPhase.ResolveDamage });

				PhaseStarting(this, new PhaseEventArgs { Phase = ResolutionPhase.MoveThreats });
				ThreatController.MoveThreats(CurrentTurn);
				PhaseEnded(this, new PhaseEventArgs { Phase = ResolutionPhase.MoveThreats });

				PerformEndOfTurn();

				var isSecondTurnOfPhase = phaseStartTurns.Contains(CurrentTurn - 1);
				if (isSecondTurnOfPhase)
					CheckForComputer();

				var isEndOfPhase = phaseStartTurns.Contains(CurrentTurn + 1);
				if (isEndOfPhase)
					PerformEndOfPhase();

				if (CurrentTurn == NumberOfTurns - 1)
					PerformEndOfGame();

				PhaseStarting(this, new PhaseEventArgs { Phase = ResolutionPhase.EndTurn });
				PhaseEnded(this, new PhaseEventArgs { Phase = ResolutionPhase.EndTurn });

				CurrentTurn++;
			}
			catch (LoseException loseException)
			{
				GameStatus = GameStatus.Lost;
				KilledBy = loseException.Threat.DisplayName;
				throw;
			}
		}

		private void PerformEndOfGame()
		{
			ThreatController.MoveThreats(CurrentTurn + 1);
			var rocketFiredLastTurn = SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.RocketFiredLastTurn;
			if (rocketFiredLastTurn != null)
				ResolveDamage(new [] {rocketFiredLastTurn.PerformAttack(null)}, null);
			var playersInFarInterceptors = SittingDuck.InterceptorStations
				.Where(station => station.StationLocation.DistanceFromShip() > 1)
				.SelectMany(station => station.Players);
			foreach(var player in playersInFarInterceptors)
			{
				player.IsKnockedOut = true;
				player.BattleBots.IsDisabled = true;
			}
			CalculateScore();
			ThreatController.OnJumpingToHyperspace();
			GameStatus = GameStatus.Won;
		}

		private void CalculateScore()
		{
			TotalPoints += SittingDuck.WhiteZone.LowerWhiteStation.VisualConfirmationComponent.TotalVisualConfirmationPoints;
			TotalPoints += ThreatController.TotalThreatPoints;
			TotalPoints += Players.Sum(player => player.BonusPoints);
		}

		private void PerformEndOfPhase()
		{
			SittingDuck.WhiteZone.LowerWhiteStation.VisualConfirmationComponent.PerformEndOfPhase();
			SittingDuck.WhiteZone.UpperWhiteStation.ComputerComponent.PerformEndOfPhase();
		}

		private void CheckForComputer()
		{
			PhaseStarting(this, new PhaseEventArgs { Phase = ResolutionPhase.ComputerCheck });
			if (!SittingDuck.WhiteZone.UpperWhiteStation.ComputerComponent.MaintenancePerformedThisPhase)
				foreach (var player in Players)
					player.Shift(CurrentTurn + 1);
			PhaseEnded(this, new PhaseEventArgs { Phase = ResolutionPhase.ComputerCheck });
		}

		private IEnumerable<PlayerInterceptorDamage> GetInterceptorDamage()
		{
			var interceptorDamages = SittingDuck.InterceptorStations
				.Select(station => station.PlayerInterceptorDamage)
				.Where(damage => damage != null);
			return interceptorDamages;
		}

		private List<PlayerDamage> GetStandardDamage()
		{
			var damages = SittingDuck.StandardStationsByLocation.Values
				.Select(station => station.CurrentPlayerDamage())
				.Where(damageList => damageList != null)
				.SelectMany(damageList => damageList.ToList())
				.ToList();
			var rocketFiredLastTurn = SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.RocketFiredLastTurn;
			if (rocketFiredLastTurn != null)
				damages.Add(rocketFiredLastTurn.PerformAttack(null));
			if (!TargetingAssistanceProvided)
				damages = damages.Where(damage => !damage.RequiresTargetingAssistance).ToList();
			return damages;
		}

		private bool TargetingAssistanceProvided
		{
			get
			{
				var anyPlayersInInterceptors = SittingDuck.InterceptorStations.SelectMany(station => station.Players).Any();
				var visualConfirmationPerformed = SittingDuck.WhiteZone.LowerWhiteStation.VisualConfirmationComponent.NumberOfConfirmationsThisTurn > 0;
				var targetingAssistanceProvided = anyPlayersInInterceptors || visualConfirmationPerformed;
				return targetingAssistanceProvided;
			}
		}

		private void PerformPlayerActions()
		{
			var playerOrder = Players
				.Where(player => !player.IsKnockedOut)
				.OrderBy(player => player.IsPerformingMedic(CurrentTurn))
				.ThenBy(player => player.Index);

			foreach (var player in playerOrder)
				player.CurrentStation.PerformPlayerAction(player, CurrentTurn);
			ThreatController.OnPlayerActionsEnded();
		}

		private void CheckForAdvancedSpecialOpsProtection()
		{
			var playersPerformingAdvancedSpecialOps = Players
				.Where(player => player.IsPerformingAdvancedSpecialOps(CurrentTurn))
				.ToList();
			if (playersPerformingAdvancedSpecialOps.Any())
				playersPerformingAdvancedSpecialOps.Single().HasSpecialOpsProtection = true;
		}

		private void PerformEndOfTurn()
		{
			foreach (var zone in SittingDuck.Zones)
			{
				zone.Gravolift.PerformEndOfTurn();
				zone.UpperStation.PerformEndOfTurn();
				zone.LowerStation.PerformEndOfTurn();
				foreach (var player in Players)
					player.SetPreventsKnockOut(false);
			}
			SittingDuck.WhiteZone.LowerWhiteStation.VisualConfirmationComponent.PerformEndOfTurn();
			SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.PerformEndOfTurn();
			foreach (var interceptorStation in SittingDuck.InterceptorStations)
				interceptorStation.PerformEndOfTurn();
			ThreatController.PerformEndOfTurn();
		}

		private void ResolveDamage(
			IEnumerable<PlayerDamage> damages,
			IEnumerable<PlayerInterceptorDamage> interceptorDamages)
		{
			if (!ThreatController.DamageableExternalThreats.Any())
				return;
			var damagesByThreat = new Dictionary<ExternalThreat, IList<PlayerDamage>>();
			foreach (var damage in damages)
			{
				var priorityThreatsInRange = ThreatController.DamageableExternalThreats
					.Where(threat =>
						threat.IsPriorityTargetFor(damage) &&
						threat.CanBeTargetedBy(damage))
					.ToList();
				var threatsInRange = ThreatController.DamageableExternalThreats
					.Where(threat => threat.CanBeTargetedBy(damage))
					.ToList();
				switch (damage.PlayerDamageType.DamageTargetType())
				{
					case DamageTargetType.All:
						foreach (var threat in threatsInRange)
							AddToDamagesByThreat(threat, damage, damagesByThreat);
						break;
					case DamageTargetType.Single:
						var priorityThreatHit = priorityThreatsInRange
							.OrderBy(threat => threat.TimeAppears)
							.FirstOrDefault();
						if (priorityThreatHit != null)
							AddToDamagesByThreat(priorityThreatHit, damage, damagesByThreat);
						else
						{
							var threatHit = threatsInRange
								.OrderBy(threat => threat.Position)
								.ThenBy(threat => threat.TimeAppears)
								.FirstOrDefault();
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

		private void AddInterceptorDamages(
			PlayerInterceptorDamage interceptorDamages,
			Dictionary<ExternalThreat, IList<PlayerDamage>> damagesByThreat)
		{
			var threatsHitByInterceptors = ThreatController.DamageableExternalThreats
				.Where(threat => threat.CanBeTargetedBy(interceptorDamages.MultipleDamage))
				.ToList();
			if (threatsHitByInterceptors.Any())
			{
				var damageType = threatsHitByInterceptors.Count == 1 ?
					interceptorDamages.SingleDamage :
					interceptorDamages.MultipleDamage;
				foreach (var threat in threatsHitByInterceptors)
					AddToDamagesByThreat(threat, damageType, damagesByThreat);
			}
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
