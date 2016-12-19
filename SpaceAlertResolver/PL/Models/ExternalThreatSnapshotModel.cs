using BLL.ShipComponents;
using BLL.Threats.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class ExternalThreatSnapshotModel : ThreatSnapshotModel
	{
		public int Shields { get;  }
		[JsonConverter(typeof(StringEnumConverter))]
		public ZoneLocation CurrentZone { get; }

		public ExternalThreatSnapshotModel(ExternalThreat threat) : base(threat)
		{
			Shields = threat.Shields;
			CurrentZone = threat.CurrentZone;
		}
	}
}
