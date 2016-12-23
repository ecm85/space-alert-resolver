using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public abstract class ZoneModel
	{
		public ZoneLocation ZoneLocation { get; set; }
		public IEnumerable<ThreatModel> ExternalThreats { get; set; }
		public TrackSnapshotModel Track { get; }
		public StandardStationModel LowerStation { get; set; }
		public StandardStationModel UpperStation { get; set; }

		protected ZoneModel(Game game, ZoneLocation zoneLocation)
		{
			ZoneLocation = zoneLocation;
			var externalThreatsInZone = game.ThreatController.ExternalThreatsOnTrack
				.Where(threat => threat.CurrentZone == zoneLocation)
				.ToList();
			ExternalThreats = externalThreatsInZone
				.Select(threat => new ExternalThreatModel(threat))
				.ToList();
			Track = new TrackSnapshotModel(game.ThreatController.ExternalTracks[zoneLocation], externalThreatsInZone);
		}
	}
}
