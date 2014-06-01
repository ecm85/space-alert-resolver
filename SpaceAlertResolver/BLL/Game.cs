using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Tracks;

namespace BLL
{
	public class Game
	{
		//TODO: Maintain list of internal threats, move on each turn
		//TODO: Don't remove threats after defeated?
		private readonly IList<ExternalThreat> allExternalThreats;
		private readonly IDictionary<Zone, ExternalTrack> externalTracks;
		private readonly SittingDuck sittingDuck;
		private readonly IList<Player> players;
		public readonly List<Threat> defeatedThreats = new List<Threat>();
		public readonly List<Threat> survivedThreats = new List<Threat>();
		private int nextTurn;
		public const int NumberOfTurns = 12;
		private readonly IList<int> phaseStartTurns = new[] {1, 4, 8};
		private readonly List<ExternalThreat> currentExternalThreats;

		public Game(SittingDuck sittingDuck, IList<ExternalThreat> allExternalThreats, IEnumerable<ExternalTrack> externalTracks, IList<Player> players)
		{
			this.sittingDuck = sittingDuck;
			this.allExternalThreats = allExternalThreats;
			this.externalTracks = externalTracks.ToDictionary(track => track.Zone);
			this.players = players;
			nextTurn = 1;
			currentExternalThreats = new List<ExternalThreat>();
		}

		public void PerformTurn()
		{
			var currentTurn = nextTurn;
			AddNewThreatsToTracks(currentTurn);
			PerformPlayerActionsAndResolveDamage(currentTurn);
			MoveThreats();
			if (currentTurn == NumberOfTurns)
			{
				MoveThreats();
				//TODO: Calculate score
				//TODO: Call JumpingToHyperspace on current threats
			}
			PerformEndOfTurn();
			var isSecondTurnOfPhase = phaseStartTurns.Contains(currentTurn - 1);
			if (isSecondTurnOfPhase)
				CheckForComputer(currentTurn);
			var isEndOfPhase = phaseStartTurns.Contains(currentTurn + 1);
			if (isEndOfPhase)
				PerformEndOfPhase();
			nextTurn++;
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
					player.Shift(currentTurn);
		}

		private void AddNewThreatsToTracks(int currentTurn)
		{
			foreach (var newThreat in allExternalThreats.Where(threat => threat.TimeAppears == currentTurn))
			{
				var track = externalTracks[newThreat.CurrentZone];
				track.AddThreat(newThreat);
				newThreat.Track = track;
				currentExternalThreats.Add(newThreat);
			}
		}

		private void MoveThreats()
		{
			foreach (var externalThreat in currentExternalThreats)
				externalTracks[externalThreat.CurrentZone].MoveThreat(externalThreat);
			foreach (var track in externalTracks.Values)
			{
				var newlySurvivedThreats = track.ThreatsSurvived;
				foreach (var survivedThreat in newlySurvivedThreats)
					currentExternalThreats.Remove(survivedThreat);
				track.RemoveThreats(newlySurvivedThreats);
				survivedThreats.AddRange(newlySurvivedThreats);
			}
		}

		private void PerformPlayerActionsAndResolveDamage(int currentTurn)
		{
			var damages = new List<PlayerDamage>();
			foreach (var player in players.Where(player => !player.IsKnockedOut))
			{
				if (player.Actions.Count >= currentTurn)
				{
					var playerAction = player.Actions[currentTurn - 1];
					switch (playerAction)
					{
						case PlayerAction.A:
							var damage = player.CurrentStation.PerformAAction(player, currentTurn);
							if (damage != null)
								damages.Add(damage);
							break;
						case PlayerAction.B:
							player.CurrentStation.PerformBAction(player, currentTurn);
							break;
						case PlayerAction.C:
							var cResult = player.CurrentStation.PerformCAction(player, currentTurn);
							//TODO: Use cResult
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
							if (currentZone.Gravolift.Occupied)
								player.Shift(currentTurn);
							else
								currentZone.Gravolift.Occupied = true;
							break;
						case PlayerAction.BattleBots:
							if (!player.BattleBots.IsDisabled)
							{
								var result = player.CurrentStation.UseBattleBots(player, currentTurn);
								player.BattleBots.IsDisabled = result.BattleBotsDisabled;
							}
							break;
					}
				}
			}
			var rocketFiredLastTurn = sittingDuck.RocketsComponent.RocketFiredLastTurn;
			if (rocketFiredLastTurn != null)
				damages.Add(rocketFiredLastTurn.PerformAttack());
			ResolveDamage(damages);
		}

		public void PerformEndOfTurn()
		{
			foreach (var zone in sittingDuck.Zones)
			{
				zone.Gravolift.Occupied = false;
				zone.UpperStation.Cannon.PerformEndOfTurn();
				zone.LowerStation.Cannon.PerformEndOfTurn();
			}
			sittingDuck.VisualConfirmationComponent.PerformEndOfTurn();
			sittingDuck.RocketsComponent.PerformEndOfTurn();
		}

		private static void MovePlayer(IStation newDestination, Player player)
		{
			var newStation = newDestination ?? player.CurrentStation;
			var oldStation = player.CurrentStation;
			player.CurrentStation = newStation;
			oldStation.Players.Remove(player);
			newStation.Players.Add(player);
		}

		private void ResolveDamage(IEnumerable<PlayerDamage> damages)
		{
			if (!currentExternalThreats.Any())
				return;
			var damagesByThreat = new Dictionary<ExternalThreat, IList<PlayerDamage>>();
			foreach (var damage in damages)
			{
				var threatsInRange = currentExternalThreats.Where(threat => threat.CanBeTargetedBy(damage)).ToList();
				switch (damage.DamageType.DamageTargetType())
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
			foreach (var threat in damagesByThreat.Keys)
				threat.TakeDamage(damagesByThreat[threat]);
			
			var newlyDefeatedThreats = currentExternalThreats.Where(externalThreat => externalThreat.RemainingHealth <= 0).ToList();
			foreach (var defeatedThreat in newlyDefeatedThreats)
				currentExternalThreats.Remove(defeatedThreat);
			foreach (var track in externalTracks.Values)
				track.RemoveThreats(newlyDefeatedThreats);
			defeatedThreats.AddRange(newlyDefeatedThreats);
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
