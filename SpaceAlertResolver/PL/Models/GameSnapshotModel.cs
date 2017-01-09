using System.Collections.Generic;
using System.Linq;
using BLL;

namespace PL.Models
{
	public class GameSnapshotModel
	{
		public IEnumerable<InternalThreatModel> InternalThreats { get; }
		public IEnumerable<PlayerModel> Players { get; }
		public TrackSnapshotModel InternalTrack { get; }
		public ZoneModel RedZone { get; set; }
		public ZoneModel WhiteZone { get; set; }
		public ZoneModel BlueZone { get; set; }

		public string Description { get; }
		public int Turn { get; }
		public int Phase { get; set; }

		public GameSnapshotModel(Game game, ResolutionPhase phase)
		{
			var internalThreats = game.ThreatController.InternalThreatsOnTrack.ToList();
			RedZone = new RedZoneModel(game);
			WhiteZone = new WhiteZoneModel(game);
			BlueZone = new BlueZoneModel(game);
			InternalThreats = internalThreats.Select(threat => new InternalThreatModel(threat)).ToList();
			Players = game.Players.Select(player => new PlayerModel(player)).ToList();
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack, internalThreats);
			Description = game.HasLost ? phase.GetDescription() + " - Lost!" : phase.GetDescription();
			Turn = game.CurrentTurn;
		}
	}
}
