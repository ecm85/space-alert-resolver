using System.Collections.Generic;
using System.Linq;
using BLL.Threats;

namespace PL.Models
{
	public class AllThreatsModel
	{
		public ThreatsByTypeModel WhiteThreats { get; set; }
		public ThreatsByTypeModel YellowThreats { get; set; }
		public ThreatsByTypeModel RedThreats { get; set; }

		public AllThreatsModel(IEnumerable<Threat> allThreats)
		{
			var threatsGroupedByColor = allThreats.GroupBy(threat => threat.Difficulty).ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
			WhiteThreats= new ThreatsByTypeModel(threatsGroupedByColor[ThreatDifficulty.White]);
			YellowThreats = new ThreatsByTypeModel(threatsGroupedByColor[ThreatDifficulty.Yellow]);
			RedThreats = new ThreatsByTypeModel(threatsGroupedByColor[ThreatDifficulty.Red]);
		}
	}
}
