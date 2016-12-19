using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class GameSnapshotModel
	{
		public IEnumerable<ExternalThreatSnapshotModel> ExternalThreats { get; }
		public IEnumerable<InternalThreatSnapshotModel> InternalThreats { get; }
		public IEnumerable<PlayerSnapshotModel> Players { get; }
		public TrackSnapshotModel RedTrack { get; }
		public TrackSnapshotModel WhiteTrack { get; }
		public TrackSnapshotModel BlueTrack { get; }
		public TrackSnapshotModel InternalTrack { get; }
		public string Description { get; }
		public int Turn { get; }
		public GameSnapshotModel(Game game, string description)
		{
			ExternalThreats = game.ThreatController.ExternalThreatsOnTrack.Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			InternalThreats = game.ThreatController.InternalThreatsOnTrack.Select(threat => new InternalThreatSnapshotModel(threat)).ToList();
			Players = game.Players.Select(player => new PlayerSnapshotModel(player)).ToList();
			RedTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Red]);
			WhiteTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.White]);
			BlueTrack = new TrackSnapshotModel(game.ThreatController.ExternalTracks[ZoneLocation.Blue]);
			InternalTrack = new TrackSnapshotModel(game.ThreatController.InternalTrack);
			Description = description;
			Turn = game.CurrentTurn;
		}
	}
}
