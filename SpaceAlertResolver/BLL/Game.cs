using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace BLL
{
	public class Game
	{
		//TODO: Variable-range interceptors (see scattered todos)
		//TODO: Specializations
		//TODO: Yellow threats
		//TODO: Red threats
		//TODO: Double actions
		//TODO: Campaign repairs and damage carryover
		//TODO: Let user select damage tokens
		//TODO: include penalties in score, and break score up more?
		private readonly IList<ExternalThreat> allExternalThreats;
		private readonly IList<InternalThreat> allInternalThreats;
		private readonly IDictionary<Zone, ExternalTrack> externalTracks;
		private readonly InternalTrack internalTrack;
		private readonly SittingDuck sittingDuck;
		private readonly IList<Player> players;
		public readonly List<Threat> defeatedThreats = new List<Threat>();
		public readonly List<Threat> survivedThreats = new List<Threat>();
		private int nextTurn;
		public const int NumberOfTurns = 12;
		private readonly IList<int> phaseStartTurns = new[] {1, 4, 8};
		public int TotalPoints { get; private set; }
		public bool AllowVariableRangeInteceptors { get; set; }

		public Game(
			SittingDuck sittingDuck,
			IList<ExternalThreat> externalThreats,
			IEnumerable<ExternalTrack> externalTracks,
			IList<InternalThreat> internalThreats,
			InternalTrack internalTrack,
			IList<Player> players)
		{
			this.sittingDuck = sittingDuck;
			allExternalThreats = externalThreats;
			allInternalThreats = internalThreats;
			this.externalTracks = externalTracks.ToDictionary(track => track.Zone);
			this.internalTrack = internalTrack;
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
			AddNewThreatsToTracks(currentTurn);
			PerformPlayerActionsAndResolveDamage(currentTurn);
			MoveThreats();
			PerformEndOfTurn();
			var isSecondTurnOfPhase = phaseStartTurns.Contains(currentTurn - 1);
			if (isSecondTurnOfPhase)
				CheckForComputer(currentTurn);
			var isEndOfPhase = phaseStartTurns.Contains(currentTurn + 1);
			if (isEndOfPhase)
				PerformEndOfPhase();
			if (currentTurn == NumberOfTurns - 1)
			{
				MoveThreats();
				var rocketFiredLastTurn = sittingDuck.RocketsComponent.RocketFiredLastTurn;
				if (rocketFiredLastTurn != null)
					ResolveDamage(new [] {rocketFiredLastTurn.PerformAttack()}, null);
				CalculateScore();
				foreach (var threat in sittingDuck.CurrentExternalThreats)
					threat.OnJumpingToHyperspace();
				foreach (var threat in sittingDuck.CurrentInternalThreats)
					threat.OnJumpingToHyperspace();
			}
			nextTurn++;
		}

		private void CalculateScore()
		{
			TotalPoints += sittingDuck.VisualConfirmationComponent.TotalVisualConfirmationPoints;
			TotalPoints += survivedThreats.Sum(threat => threat.PointsForSurviving);
			TotalPoints += defeatedThreats.Sum(threat => threat.PointsForDefeating);
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

		private void AddNewThreatsToTracks(int currentTurn)
		{
			foreach (var newThreat in allExternalThreats.Where(threat => threat.TimeAppears == currentTurn))
			{
				var track = externalTracks[newThreat.CurrentZone];
				track.AddThreat(newThreat);
				newThreat.SetTrack(track);
				sittingDuck.CurrentExternalThreats.Add(newThreat);
			}
			foreach (var newThreat in allInternalThreats.Where(threat => threat.TimeAppears == currentTurn))
			{
				internalTrack.AddThreat(newThreat);
				sittingDuck.CurrentInternalThreats.Add(newThreat);
			}
		}

		private void MoveThreats()
		{
			var moveCallByThreat = new Dictionary<Threat, Action>();
			foreach (var externalThreat in sittingDuck.CurrentExternalThreats)
				moveCallByThreat[externalThreat] = GetMoveCall(externalThreat);
			foreach (var internalThreat in sittingDuck.CurrentInternalThreats)
				moveCallByThreat[internalThreat] = GetMoveCall(internalThreat);
			var allCurrentThreats = new List<Threat>()
				.Concat(sittingDuck.CurrentExternalThreats)
				.Concat(sittingDuck.CurrentInternalThreats)
				.OrderBy(threat => threat.TimeAppears);
			foreach (var threat in allCurrentThreats)
				moveCallByThreat[threat]();

			foreach (var track in externalTracks.Values)
				RemoveSurvivedThreats(sittingDuck.CurrentExternalThreats, track);
			AddMalfunctionsForSurvivedInternalThreats();
			RemoveSurvivedThreats(sittingDuck.CurrentInternalThreats, internalTrack);
		}

		private Action GetMoveCall(InternalThreat internalThreat)
		{
			return () => internalTrack.MoveThreat(internalThreat);
		}

		private Action GetMoveCall(ExternalThreat externalThreat)
		{
			return () => externalTracks[externalThreat.CurrentZone].MoveThreat(externalThreat);
		}

		private void AddMalfunctionsForSurvivedInternalThreats()
		{
			var newlySurvivedThreats = internalTrack.ThreatsSurvived;
			foreach (var threat in newlySurvivedThreats)
			{
				foreach(var station in threat.CurrentStations)
					station.IrreparableMalfunctions.Add(threat.GetIrreparableMalfunction());
			}
		}

		private void RemoveSurvivedThreats<T>(IList<T> currentThreats, Track<T> track) where T : Threat
		{
			var newlySurvivedThreats = track.ThreatsSurvived;
			foreach (var survivedThreat in newlySurvivedThreats)
				currentThreats.Remove(survivedThreat);
			track.RemoveThreats(newlySurvivedThreats);
			survivedThreats.AddRange(newlySurvivedThreats);
		}

		private void PerformPlayerActionsAndResolveDamage(int currentTurn)
		{
			var damages = new List<PlayerDamage>();
			foreach (var player in players.Where(player => !player.IsKnockedOut))
			{
				var playerAction = player.Actions[currentTurn];
				PerformPlayerAction(currentTurn, playerAction, player, damages);
				RemoveDefeatedInternalThreats();
			}
			foreach (var threat in sittingDuck.CurrentInternalThreats)
				threat.PerformEndOfPlayerActions();

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
					var currentZone = sittingDuck.ZonesByLocation[player.CurrentStation.ZoneLocation];
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
			if (!sittingDuck.CurrentExternalThreats.Any())
				return;
			var damagesByThreat = new Dictionary<ExternalThreat, IList<PlayerDamage>>();
			foreach (var damage in damages)
			{
				var threatsInRange = sittingDuck.CurrentExternalThreats.Where(threat => threat.CanBeTargetedBy(damage)).ToList();
				switch (damage.PlayerDamageType.DamageTargetType())
				{
					case DamageTargetType.All:
						foreach (var threat in threatsInRange)
							AddToDamagesByThreat(threat, damage, damagesByThreat);
						break;
					case DamageTargetType.Single:
						var threatHit = threatsInRange.OrderBy(threat => threat.TrackPosition).ThenBy(threat => threat.TimeAppears).FirstOrDefault();
						if (threatHit != null)
							AddToDamagesByThreat(threatHit, damage, damagesByThreat);
						break;
					default:
						throw new InvalidOperationException();
				}
			}
			if (interceptorDamages != null)
				AddInterceptorDamages(interceptorDamages, damagesByThreat);

			foreach (var threat in damagesByThreat.Keys)
				threat.TakeDamage(damagesByThreat[threat]);

			RemoveDefeatedExternalThreats();
		}

		private void RemoveDefeatedInternalThreats()
		{
			var newlyDefeatedThreats = sittingDuck.CurrentInternalThreats.Where(externalThreat => externalThreat.RemainingHealth <= 0).ToList();
			foreach (var defeatedThreat in newlyDefeatedThreats)
				sittingDuck.CurrentInternalThreats.Remove(defeatedThreat);
			internalTrack.RemoveThreats(newlyDefeatedThreats);
			defeatedThreats.AddRange(newlyDefeatedThreats);
		}

		private void RemoveDefeatedExternalThreats()
		{
			var newlyDefeatedThreats = sittingDuck.CurrentExternalThreats.Where(externalThreat => externalThreat.RemainingHealth <= 0).ToList();
			foreach (var defeatedThreat in newlyDefeatedThreats)
				sittingDuck.CurrentExternalThreats.Remove(defeatedThreat);
			foreach (var track in externalTracks.Values)
				track.RemoveThreats(newlyDefeatedThreats);
			defeatedThreats.AddRange(newlyDefeatedThreats);
		}

		private void AddInterceptorDamages(PlayerInterceptorDamage interceptorDamages, Dictionary<ExternalThreat, IList<PlayerDamage>> damagesByThreat)
		{
			var interceptorDamagesMultiple = interceptorDamages.MultipleDamage;
			var threatsHitByInterceptors =
				sittingDuck.CurrentExternalThreats.Where(threat => threat.CanBeTargetedBy(interceptorDamagesMultiple)).ToList();
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
