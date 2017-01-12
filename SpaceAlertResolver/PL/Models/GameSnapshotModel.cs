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

		public string Description { get; }
		public int Turn { get; }
		public int Phase { get; set; }

		public GameSnapshotModel(Game game, ResolutionPhase phase)
		{
			var internalThreats = game.ThreatController.InternalThreatsOnTrack.ToList();
			RedZone = new RedZoneModel(game);
			WhiteZone = new WhiteZoneModel(game);
			BlueZone = new BlueZoneModel(game);
			InterceptorsZone = new InterceptorsZoneModel(game);
			InternalThreats = internalThreats.Select(threat => new InternalThreatModel(threat)).ToList();
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack, internalThreats);
			Description = phase.GetDescription();
			Turn = game.CurrentTurn;
			KilledBy = game.KilledBy;
			GameStatus = game.GameStatus.GetDisplayName();
		}
	}
}
