using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Tracks;

namespace BLL
{
	public class Game
	{
		//TODO: Maintain list of internal threats, move on each turn
		//TODO: Don't remove threats after defeated?
		private readonly IList<ExternalThreat> threats;
		private readonly IDictionary<Zone, Track> tracks;
		private readonly SittingDuck sittingDuck;
		private readonly IList<Player> players;
		private readonly List<ExternalThreat> defeatedThreats = new List<ExternalThreat>();
		private int nextTurn;
		private readonly int numberOfTurns;

		public Game(SittingDuck sittingDuck, IList<ExternalThreat> threats, IEnumerable<Track> tracks, IList<Player> players, int numberOfTurns)
		{
			this.sittingDuck = sittingDuck;
			this.threats = threats;
			this.tracks = tracks.ToDictionary(track => track.Zone);
			this.players = players;
			this.numberOfTurns = numberOfTurns;
			nextTurn = 1;
		}

		public void PerformTurn()
		{
			//TODO: Handle computer
			var currentTurn = nextTurn;
			AddNewThreatsToTracks(currentTurn);
			PerformPlayerActionsAndResolveDamage(currentTurn);
			MoveThreats();
			if (currentTurn == numberOfTurns)
			{
				MoveThreats();
				//TODO: Calculate score
				//TODO: Call JumpingToHyperspace on current threats
			}
			nextTurn++;
		}

		private void AddNewThreatsToTracks(int currentTurn)
		{
			foreach (var newThreat in threats.Where(threat => threat.TimeAppears == currentTurn))
				tracks[newThreat.CurrentZone].AddThreat(newThreat);
		}

		private IEnumerable<ExternalThreat> CurrentThreats
		{
			get { return tracks.SelectMany(track => track.Value.ThreatsOnTrack).OrderBy(threat => threat.TimeAppears).ToList(); }
		}

		private void MoveThreats()
		{
			foreach (var externalThreat in CurrentThreats)
				tracks[externalThreat.CurrentZone].MoveThreat(externalThreat, sittingDuck);
		}

		private void PerformPlayerActionsAndResolveDamage(int currentTurn)
		{
			var damages = new List<PlayerDamage>();
			foreach (var player in players.Where(player => !player.IsKnockedOut))
			{
				var playerAction = player.Actions[currentTurn - 1];
				switch (playerAction)
				{
					case PlayerAction.A:
						//TODO: Don't let a gun fire twice
						var damage = player.CurrentStation.PerformAAction();
						if (damage != null)
							damages.Add(damage);
						break;
					case PlayerAction.B:
						player.CurrentStation.PerformBAction();
						break;
					case PlayerAction.C:
						var cResult = player.CurrentStation.PerformCAction(player);
						//TODO: Use cResult
						break;
					case PlayerAction.MoveBlue:
						MovePlayer(player.CurrentStation.BluewardStation, player);
						break;
					case PlayerAction.MoveRed:
						MovePlayer(player.CurrentStation.RedwardStation, player);
						break;
					case PlayerAction.ChangeDeck:
						//TODO: Handle multiple people in lift
						MovePlayer(player.CurrentStation.OppositeDeckStation, player);
						break;
					case PlayerAction.BattleBots:
						if (!player.BattleBots.IsDisabled)
						{
							var result = player.CurrentStation.UseBattleBots();
							player.BattleBots.IsDisabled = result.BattleBotsDisabled;
						}
						break;
				}
			}
			ResolveDamage(damages);
		}

		private static void MovePlayer(IStation newDestination, Player player)
		{
			var newStation = newDestination ?? player.CurrentStation;
			var oldStation = player.CurrentStation;
			player.CurrentStation = newStation;
			oldStation.Players.Remove(player);
			newStation.Players.Add(player);
		}

		private void ResolveDamage(IList<PlayerDamage> damages)
		{
			var alreadyHitThreats = HitClosestThreats(damages);
			HitAllThreatsWithPulse(damages, alreadyHitThreats);
			defeatedThreats.AddRange(tracks.Values.SelectMany(track => track.RemoveDefeatedThreats()));
		}

		private void HitAllThreatsWithPulse(IEnumerable<PlayerDamage> damages, IEnumerable<ExternalThreat> alreadyHitThreats)
		{
			//Only one pulse cannon is currently supported.
			var pulseDamage = damages.SingleOrDefault(damage => damage.DamageType == DamageType.Pulse);
			if (pulseDamage == null)
				return;
			var allThreatsInPulseRange =
				tracks.Values.SelectMany(track => track.GetThreatsWithinDistance(pulseDamage.Range)).Except(alreadyHitThreats);
			foreach (var externalThreat in allThreatsInPulseRange)
				externalThreat.TakeDamage(new[] {pulseDamage});
		}

		private IEnumerable<ExternalThreat> HitClosestThreats(IList<PlayerDamage> damages)
		{
			var alreadyHitThreats = new List<ExternalThreat>();
			foreach (var zone in sittingDuck.Zones)
			{
				var track = tracks[zone];
				var closestThreatInZone = track.ClosestThreat();
				if (closestThreatInZone != null)
				{
					var distanceToThreat = track.DistanceToThreat(closestThreatInZone);
					closestThreatInZone.TakeDamage(
						damages.Where(damage => damage.Range >= distanceToThreat && damage.ZoneLocations.Contains(zone.ZoneLocation)).ToList());
					alreadyHitThreats.Add(closestThreatInZone);
				}
			}
			return alreadyHitThreats;
		}
	}
}
