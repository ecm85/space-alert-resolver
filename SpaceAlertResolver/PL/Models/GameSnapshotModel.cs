using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.Threats.Internal;

namespace PL.Models
{
	public class GameSnapshotModel
	{
		public IEnumerable<InternalThreatModel> InternalThreats { get; }
		public TrackSnapshotModel InternalTrack { get; }
		public StandardZoneModel RedZone { get; set; }
		public StandardZoneModel WhiteZone { get; set; }
		public StandardZoneModel BlueZone { get; set; }
		public InterceptorsZoneModel InterceptorsZone { get; set; }
		public string KilledBy { get; set; }
		public string GameStatus { get; set; }

		public IEnumerable<ThreatModel> DefeatedThreats { get; set; }
		public IEnumerable<ThreatModel> SurvivedThreats { get; set; }
		public int TotalDefeatedPoints { get { return DefeatedThreats.Sum(threat => threat.Points); } }
		public int TotalSurvivedPoints { get { return SurvivedThreats.Sum(threat => threat.Points); } }

		public IEnumerable<PlayerModel> KnockedOutPlayers { get; set; }

		public string PhaseDescription { get; }
		public string PhaseSubDescription { get; }
		public int TurnNumber { get; }

		public bool IsSubPhase { get; }

		public IEnumerable<PlayerModel> Players { get; }

		public GameSnapshotModel(Game game, string phaseDescription, string phaseSubDescription, bool isSubPhase = false)
		{
			IsSubPhase = isSubPhase;
			var internalThreats = game.ThreatController.InternalThreatsOnTrack.ToList();
			var parentThreats = internalThreats.Where(threat => threat.Parent == null).ToList();
			var internalThreatsOnTrack = parentThreats
				.Concat(OrphanThreatRepresentatives(internalThreats))
				.ToList();
			var threatPositions = internalThreatsOnTrack.Select(threat => threat.Position).ToList();
			RedZone = new RedZoneModel(game);
			WhiteZone = new WhiteZoneModel(game);
			BlueZone = new BlueZoneModel(game);
			InterceptorsZone = new InterceptorsZoneModel(game);
			InternalThreats = internalThreatsOnTrack.Select(threat => new InternalThreatModel(threat)).ToList();
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack, threatPositions);
			PhaseDescription = phaseDescription;
			PhaseSubDescription = phaseSubDescription;
			TurnNumber = game.CurrentTurn;
			KilledBy = game.KilledBy;
			GameStatus = game.GameStatus.GetDisplayName();
			DefeatedThreats = game.ThreatController.DefeatedThreats.Select(threat => new ThreatModel(threat)).ToList();
			SurvivedThreats = game.ThreatController.SurvivedThreats.Select(threat => new ThreatModel(threat)).ToList();
			KnockedOutPlayers = game.Players.Where(player => player.IsKnockedOut).Select(player => new PlayerModel(player)).ToList();
			Players = game.Players.Select(player => new PlayerModel(player)).ToList();
		}

		private static IEnumerable<InternalThreat> OrphanThreatRepresentatives(List<InternalThreat> internalThreats)
		{
			return internalThreats
				.Where(threat => threat.Parent != null)
				.GroupBy(threat => threat.Parent)
				.Where(group => !group.Key.IsOnTrack)
				.Select(group => group.First());
		}
	}
}
