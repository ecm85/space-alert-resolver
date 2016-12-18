using System.Collections.Generic;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace PL.Models
{
	public class InternalThreatSnapshotModel : ThreatSnapshotModel
	{
		public int TotalInaccessibility { get; set; }
		public IEnumerable<StationLocation> CurrentStations { get; set; }

		public InternalThreatSnapshotModel(InternalThreat threat) : base(threat)
		{
			TotalInaccessibility = threat.TotalInaccessibility.GetValueOrDefault();
			CurrentStations = threat.CurrentStations;
		}
	}
}