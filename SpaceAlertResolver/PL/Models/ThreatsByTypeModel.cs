using System.Collections.Generic;
using System.Linq;
using BLL.Threats;

namespace PL.Models
{
	public class ThreatsByTypeModel
	{
		public IEnumerable<ThreatModel> SeriousExternalThreats { get; set; }
		public IEnumerable<ThreatModel> SeriousInternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorExternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorInternalThreats { get; set; }

		public ThreatsByTypeModel(IEnumerable<Threat> threats)
		{
			var threatsGroupedByType = threats.GroupBy(threat => threat.ThreatType).ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
			SeriousExternalThreats = threatsGroupedByType[ThreatType.SeriousExternal].Select(threat => new ThreatModel(threat));
			SeriousInternalThreats = threatsGroupedByType[ThreatType.SeriousInternal].Select(threat => new ThreatModel(threat));
			MinorExternalThreats = threatsGroupedByType[ThreatType.MinorExternal].Select(threat => new ThreatModel(threat));
			MinorInternalThreats = threatsGroupedByType[ThreatType.MinorInternal].Select(threat => new ThreatModel(threat));
		}
	}
}
