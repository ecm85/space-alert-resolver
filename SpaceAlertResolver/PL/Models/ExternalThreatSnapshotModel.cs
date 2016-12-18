using BLL.ShipComponents;
using BLL.Threats.External;

namespace PL.Models
{
	public class ExternalThreatSnapshotModel : ThreatSnapshotModel
	{
		public int Shields { get;  }
		public ZoneLocation CurrentZone { get; }

		public ExternalThreatSnapshotModel(ExternalThreat threat) : base(threat)
		{
			Shields = threat.Shields;
			CurrentZone = threat.CurrentZone;
		}
	}
}
