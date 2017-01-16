using BLL.ShipComponents;
using BLL.Threats.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class ExternalThreatModel : ThreatModel
	{
		public int Shields { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public ZoneLocation CurrentZone { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public ExternalThreatModel(ExternalThreat threat) : base(threat)
		{
			Shields = threat.Shields;
			CurrentZone = threat.CurrentZone;
			Id = ExternalThreatFactory.ThreatIdsByType[threat.GetType()];
			Name = ExternalThreatFactory.ThreatNamesByType[threat.GetType()];
			Description = threat.GetType().Name;
		}

		[JsonConstructor]
		public ExternalThreatModel()
		{
		}
	}
}
