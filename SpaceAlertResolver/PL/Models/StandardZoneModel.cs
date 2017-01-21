using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
	public abstract class StandardZoneModel
	{
		public ZoneLocation ZoneLocation { get; set; }
		public IEnumerable<ThreatModel> ExternalThreats { get; set; }
		public TrackSnapshotModel Track { get; }
		public StandardStationModel LowerStation { get; set; }
		public StandardStationModel UpperStation { get; set; }
		public IEnumerable<string> Damage { get; set; }
		public int TotalDamage { get; set; }

		protected StandardZoneModel(Game game, ZoneLocation zoneLocation)
		{
			var zone = game.SittingDuck.ZonesByLocation[zoneLocation];
			ZoneLocation = zoneLocation;
			var externalThreatsInZone = game.ThreatController.ExternalThreatsOnTrack
				.Where(threat => threat.CurrentZone == zoneLocation)
				.ToList();
			var externalThreatPositions = externalThreatsInZone
				.Select(threat => threat.Position)
				.ToList();
			ExternalThreats = externalThreatsInZone
				.Select(threat => new ExternalThreatModel(threat))
				.ToList();
			Track = new TrackSnapshotModel(game.ThreatController.ExternalTracks[zoneLocation], externalThreatPositions);
			Damage = zone.AllDamageTokensTaken
				.Select(damage => string.Format(CultureInfo.InvariantCulture, "{0}-{1}", zoneLocation, damage))
				.ToList();
			TotalDamage = zone.TotalDamage;
		}
	}
}
