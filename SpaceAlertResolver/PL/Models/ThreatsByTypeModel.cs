using System.Collections.Generic;
using System.Linq;
using BLL.Threats;

namespace PL.Models
{
	public class ThreatsByTypeModel
	{
		public IEnumerable<ThreatModel> SeriousThreats { get; set; }
		public IEnumerable<ThreatModel> MinorThreats { get; set; }

		public ThreatsByTypeModel(IEnumerable<ThreatModel> threats)
		{
			var threatsGroupedByType = threats.ToLookup(threat => threat.ThreatType);

			SeriousThreats = threatsGroupedByType[ThreatType.SeriousExternal].Concat(threatsGroupedByType[ThreatType.SeriousInternal]).ToList();
			MinorThreats = threatsGroupedByType[ThreatType.MinorExternal].Concat(threatsGroupedByType[ThreatType.MinorInternal]).ToList();
		}
	}
}
