using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Threats;
using BLL.Tracks;

namespace BLL
{
	public class Game
	{
		public EventMaster EventMaster { get; set; }
		private static readonly int[] phaseEndTurns = new[] { 3, 7, 12 };
		public SittingDuck SittingDuck { get; }
		public IList<Player> Players { get; }
		public int CurrentTurn { get; private set; }
		public int NumberOfTurns { get; set; }
		public int TotalPoints { get; private set; }
		public ThreatController ThreatController { get; }
		public GameStatus GameStatus { get; private set; }
		public string KilledBy { get; set; }

		public event EventHandler<PhaseEventArgs> PhaseStarting = (sender, args) => { };
		public event EventHandler LostGame = (sender, args) => { };
		
		public Game(
			IList<Player> players,
			IList<InternalThreat> internalThreats,
			IList<ExternalThreat> externalThreats,
			IList<Threat> bonusThreats,
			IDictionary<ZoneLocation, TrackConfiguration> externalTrackConfigurationsByZone,
			TrackConfiguration internalTrackConfiguration,
			ILookup<ZoneLocation, DamageToken> initialDamage)
		{
			EventMaster = new EventMaster();
			GameStatus = GameStatus.InProgress;
			NumberOfTurns = 12;
			var externalTracksByZone = externalTrackConfigurationsByZone.ToDictionary(
				trackConfigurationWithZone => trackConfigurationWithZone.Key,
				trackConfigurationWithZone => new Track(trackConfigurationWithZone.Value));
			var internalTrack = new Track(internalTrackConfiguration);
			ThreatController = new ThreatController(externalTracksByZone, internalTrack, externalThreats, internalThreats);
			ThreatController.PhaseStarting += (sender, args) =>  PhaseStarting(this, args);
			SittingDuck = new SittingDuck(ThreatController, this, initialDamage);
			var allThreats = bonusThreats.Concat(internalThreats).Concat(externalThreats);
			foreach (var threat in allThreats)
				threat.Initialize(SittingDuck, ThreatController, EventMaster);
			SittingDuck.SetPlayers(players);
			Players = players;
			foreach (var player in players)
				player.Initialize(SittingDuck);
			PadPlayerActions();
		}

		public void StartGame()
		{
			CurrentTurn = 1;
		}

		private void PadPlayerActions()
		{
			foreach (var player in Players)
			{
				player.PadPlayerActions(NumberOfTurns);
				
			}
		}

		public void PerformTurn()
		{
			try
			{
				ThreatController.AddNewThreatsToTracks(CurrentTurn);

				PerformPlayerActions();

				var damage = GetStandardDamage();
				var interceptorDamage = GetInterceptorDamage();
				ResolveDamage(damage, interceptorDamage);

				ThreatController.MoveThreats(CurrentTurn);

				PerformEndOfTurn();

				if (SittingDuck.WhiteZone.UpperWhiteStation.ComputerComponent.ShouldCheckComputer(CurrentTurn))
					CheckForComputer();

				var isEndOfPhase = phaseEndTurns.Contains(CurrentTurn);
				if (isEndOfPhase)
					PerformEndOfPhase();

				if (CurrentTurn == NumberOfTurns)
					PerformEndOfGame();


				CurrentTurn++;
			}
			catch (LoseException loseException)
			{
				GameStatus = GameStatus.Lost;
				KilledBy = loseException.Threat.DisplayName;
				LostGame(this, EventArgs.Empty);
			}
		}

		private void PerformEndOfGame()
		{
			ThreatController.MoveThreats(CurrentTurn + 1);
			var rocketFiredLastTurn = SittingDuck.BlueZone.LowerBlueStation.RocketsComponent.RocketFiredLastTurn;
			if (rocketFiredLastTurn != null)
				ResolveDamage(new [] {rocketFiredLastTurn.PerformAttack(null)}, new List<PlayerInterceptorDamage>());
			var playersInFarInterceptors = SittingDuck.InterceptorStations
				.Where(station => station.StationLocation.DistanceFromShip() > 1)
				.SelectMany(station => station.Players);
			foreach(var player in playersInFarInterceptors)
				player.KnockOut();
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
			PhaseStarting(this, new PhaseEventArgs { PhaseHeader = ResolutionPhase.ComputerCheck.GetDescription() });
			SittingDuck.WhiteZone.UpperWhiteStation.ComputerComponent.PerformComputerCheck(Players, CurrentTurn);
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
			PhaseStarting(this, new PhaseEventArgs
			{
				PhaseHeader = ResolutionPhase.PerformPlayerActions.GetDescription()
			});
			foreach (var player in Players)
				player.PerformStartOfPlayerActions(CurrentTurn);

			var playerOrder = Players
				.Where(player => !player.IsKnockedOut)
				.OrderByDescending(player => player.IsPerformingMedic(CurrentTurn))
				.ThenBy(player => player.Index);
			
			foreach (var player in playerOrder)
			{
				EventMaster.LogEvent(player.PlayerColor.ToString());
				while (!player.GetActionForTurn(CurrentTurn).AllActionsPerformed())
					player.CurrentStation.PerformNextPlayerAction(player, CurrentTurn);
			}
			ThreatController.OnPlayerActionsEnded();
		}

		private void PerformEndOfTurn()
		{
			foreach (var zone in SittingDuck.Zones)
			{
				zone.Gravolift.PerformEndOfTurn();
				zone.UpperStation.PerformEndOfTurn();
				zone.LowerStation.PerformEndOfTurn();
				foreach (var player in Players)
					player.PerformEndOfTurn();
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
			PhaseStarting(this, new PhaseEventArgs { PhaseHeader = ResolutionPhase.ResolveDamage.GetDescription() });
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
