using System.Collections.Generic;
using BLL.ShipComponents;
using BLL.Threats.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class InternalThreatSnapshotModel : ThreatSnapshotModel
	{
		public int TotalInaccessibility { get; set; }
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public IEnumerable<StationLocation> CurrentStations { get; set; }

		public InternalThreatSnapshotModel(InternalThreat threat) : base(threat)
		{
			TotalInaccessibility = threat.TotalInaccessibility.GetValueOrDefault();
			CurrentStations = threat.CurrentStations;
		}
	}
}