using System.Collections.Generic;
using System.Linq;
using BLL.Threats;

namespace PL.Models
{
	public class AllThreatsModel
	{
		public IEnumerable<ThreatModel> SeriousWhiteExternalThreats { get; set; }
		public IEnumerable<ThreatModel> SeriousWhiteInternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorWhiteExternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorWhiteInternalThreats { get; set; }

		public IEnumerable<ThreatModel> SeriousYellowExternalThreats { get; set; }
		public IEnumerable<ThreatModel> SeriousYellowInternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorYellowExternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorYellowInternalThreats { get; set; }

		public IEnumerable<ThreatModel> SeriousRedExternalThreats { get; set; }
		public IEnumerable<ThreatModel> SeriousRedInternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorRedExternalThreats { get; set; }
		public IEnumerable<ThreatModel> MinorRedInternalThreats { get; set; }

		public AllThreatsModel(IEnumerable<Threat> allThreats)
		{
			var threatsGroupedByColor = allThreats.GroupBy(threat => threat.Difficulty).ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
			SetWhiteThreats(threatsGroupedByColor[ThreatDifficulty.White]);
			SetYellowThreats(threatsGroupedByColor[ThreatDifficulty.Yellow]);
			SetRedThreats(threatsGroupedByColor[ThreatDifficulty.Red]);
		}

		private void SetRedThreats(List<Threat> redThreats)
		{
			var redThreatsGroupedByType = redThreats.GroupBy(threat => threat.ThreatType).ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
			SeriousRedExternalThreats = redThreatsGroupedByType[ThreatType.SeriousExternal].Select(threat => new ThreatModel(threat));
			SeriousRedInternalThreats = redThreatsGroupedByType[ThreatType.SeriousInternal].Select(threat => new ThreatModel(threat));
			MinorRedExternalThreats = redThreatsGroupedByType[ThreatType.MinorExternal].Select(threat => new ThreatModel(threat));
			MinorRedInternalThreats = redThreatsGroupedByType[ThreatType.MinorInternal].Select(threat => new ThreatModel(threat));
		}

		private void SetYellowThreats(List<Threat> yellowThreats)
		{
			var yellowThreatsGroupedByType = yellowThreats.GroupBy(threat => threat.ThreatType).ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
			SeriousYellowExternalThreats = yellowThreatsGroupedByType[ThreatType.SeriousExternal].Select(threat => new ThreatModel(threat));
			SeriousYellowInternalThreats = yellowThreatsGroupedByType[ThreatType.SeriousInternal].Select(threat => new ThreatModel(threat));
			MinorYellowExternalThreats = yellowThreatsGroupedByType[ThreatType.MinorExternal].Select(threat => new ThreatModel(threat));
			MinorYellowInternalThreats = yellowThreatsGroupedByType[ThreatType.MinorInternal].Select(threat => new ThreatModel(threat));
		}

		private void SetWhiteThreats(List<Threat> whiteThreats)
		{
			var whiteThreatsGroupedByType = whiteThreats.GroupBy(threat => threat.ThreatType)
				.ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
			SeriousWhiteExternalThreats = whiteThreatsGroupedByType[ThreatType.SeriousExternal].Select(threat => new ThreatModel(threat));
			SeriousWhiteInternalThreats = whiteThreatsGroupedByType[ThreatType.SeriousInternal].Select(threat => new ThreatModel(threat));
			MinorWhiteExternalThreats = whiteThreatsGroupedByType[ThreatType.MinorExternal].Select(threat => new ThreatModel(threat));
			MinorWhiteInternalThreats = whiteThreatsGroupedByType[ThreatType.MinorInternal].Select(threat => new ThreatModel(threat));
		}
	}
}