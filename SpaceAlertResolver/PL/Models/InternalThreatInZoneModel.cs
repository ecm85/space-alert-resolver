using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats;
using BLL.Threats.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class InternalThreatInZoneModel : ThreatModel
	{
		public int TotalInaccessibility { get; set; }
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public IEnumerable<StationLocation> CurrentStations { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public InternalThreatInZoneModel(InternalThreat threat) : base(threat)
		{
			TotalInaccessibility = threat.TotalInaccessibility.GetValueOrDefault();
			CurrentStations = threat.CurrentStations.ToList();
			var pseudoThreat = threat as IPseudoThreat;

			if (pseudoThreat != null)
			{
				Id = InternalThreatFactory.ThreatIdsByType[pseudoThreat.Parent.GetType()];
				Name = InternalThreatFactory.ThreatNamesByType[pseudoThreat.Parent.GetType()];
				Description = pseudoThreat.Parent.GetType().Name;
			}
			else
			{
				Id = InternalThreatFactory.ThreatIdsByType[threat.GetType()];
				Name = InternalThreatFactory.ThreatNamesByType[threat.GetType()];
				Description = threat.GetType().Name;
			}
		}

		[JsonConstructor]
		public InternalThreatInZoneModel()
		{
		}
	}
}
