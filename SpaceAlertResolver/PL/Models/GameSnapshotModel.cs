using System.Collections.Generic;
using System.Linq;
using BLL;

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
		public int TurnNumber { get; }

		public GameSnapshotModel(Game game, ResolutionPhase phase)
		{
			var internalThreats = game.ThreatController.InternalThreatsOnTrack.ToList();
			RedZone = new RedZoneModel(game);
			WhiteZone = new WhiteZoneModel(game);
			BlueZone = new BlueZoneModel(game);
			InterceptorsZone = new InterceptorsZoneModel(game);
			InternalThreats = internalThreats.Where(threat => threat.ShowOnTrack).Select(threat => new InternalThreatModel(threat)).ToList();
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack, internalThreats.Where(threat => threat.ShowOnTrack));
			PhaseDescription = phase.GetDescription();
			TurnNumber = game.CurrentTurn;
			KilledBy = game.KilledBy;
			GameStatus = game.GameStatus.GetDisplayName();
			DefeatedThreats = game.ThreatController.DefeatedThreats.Select(threat => new ThreatModel(threat)).ToList();
			SurvivedThreats = game.ThreatController.SurvivedThreats.Select(threat => new ThreatModel(threat)).ToList();
			KnockedOutPlayers = game.Players.Where(player => player.IsKnockedOut).Select(player => new PlayerModel(player)).ToList();
		}
	}
}
