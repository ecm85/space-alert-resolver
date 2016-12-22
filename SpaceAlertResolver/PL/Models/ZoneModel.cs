using System.Collections.Generic;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public class ZoneModel
	{
		public ZoneLocation ZoneLocation { get; set; }
		public IEnumerable<ThreatModel> ExternalThreats { get; set; }
		public StationModel LowerStation { get; set; }
		public StationModel UpperStation { get; set; }
		public TrackSnapshotModel Track { get; }

		public ZoneModel(Game game, ZoneLocation zoneLocation)
		{
			ZoneLocation = zoneLocation;
			var externalThreatsInZone = game.ThreatController.ExternalThreatsOnTrack
				.Where(threat => threat.CurrentZone == zoneLocation)
				.ToList();
			ExternalThreats = externalThreatsInZone
				.Select(threat => new ExternalThreatModel(threat))
				.ToList();
			UpperStation = new StationModel(game, zoneLocation.GetUpperStation());
			LowerStation = new StationModel(game, zoneLocation.GetLowerStation());
			Track = new TrackSnapshotModel(game.ThreatController.ExternalTracks[zoneLocation], externalThreatsInZone);
		}
	}
}
