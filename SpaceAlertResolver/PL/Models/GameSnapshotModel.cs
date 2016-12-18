using System.Collections.Generic;
using System.Linq;
using BLL;

namespace PL.Models
{
	public class GameSnapshotModel
	{
		public IEnumerable<ExternalThreatSnapshotModel> ExternalThreats { get; }
		public IEnumerable<InternalThreatSnapshotModel> InternalThreats { get; }
		public IEnumerable<PlayerSnapshotModel> Players { get; }
		public string Description { get; }
		public int Turn { get; }
		public GameSnapshotModel(Game game, string description)
		{
			ExternalThreats = game.ThreatController.ExternalThreatsOnTrack.Select(threat => new ExternalThreatSnapshotModel(threat)).ToList();
			InternalThreats = game.ThreatController.InternalThreatsOnTrack.Select(threat => new InternalThreatSnapshotModel(threat)).ToList();
			Players = game.Players.Select(player => new PlayerSnapshotModel(player)).ToList();
			Description = description;
			Turn = game.CurrentTurn;
		}
	}
}
