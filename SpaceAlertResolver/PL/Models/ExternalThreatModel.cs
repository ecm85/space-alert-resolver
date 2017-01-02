using BLL.ShipComponents;
using BLL.Threats.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class ExternalThreatModel : ThreatModel
	{
		public int Shields { get;  }
		[JsonConverter(typeof(StringEnumConverter))]
		public ZoneLocation CurrentZone { get; }
		public override string Id { get; }

		public ExternalThreatModel(ExternalThreat threat) : base(threat)
		{
			Shields = threat.Shields;
			CurrentZone = threat.CurrentZone;
			Id = ThreatFactory.ThreatIdsByType[threat.GetType()];
		}
	}
}
