using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class ThreatModel
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public ThreatType ThreatType { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public ThreatDifficulty ThreatDifficulty { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; set; }
		public int TimeAppears { get; set; }
		public string Id { get; set; }
		public string DisplayName { get; set; }
		public string FileName { get; set; }
		public int Points { get; set; }
		public int BuffCount { get; set; }
		public int DebuffCount { get; set; }
		public bool IsPhasedOut { get; set; }
		public bool NeedsBonusExternalThreat { get; set; }
		public bool NeedsBonusInternalThreat { get; set; }

		public InternalThreatModel BonusInternalThreat { get; set; }
		public ExternalThreatModel BonusExternalThreat { get; set; }

		public ThreatModel(Threat threat)
		{
			RemainingHealth = threat.RemainingHealth;
			Speed = threat.Speed;
			ThreatType = threat.ThreatType;
			ThreatDifficulty = threat.Difficulty;
			TimeAppears = threat.TimeAppears;
			Id = threat.Id;
			DisplayName = threat.DisplayName;
			FileName = threat.FileName;
			Points = threat.Points;
			BuffCount = threat.BuffCount;
			DebuffCount = threat.DebuffCount;
			IsPhasedOut = threat.GetThreatStatus(ThreatStatus.PhasedOut);
			NeedsBonusExternalThreat = threat.NeedsBonusExternalThreat;
			NeedsBonusInternalThreat = threat.NeedsBonusInternalThreat;
			BonusInternalThreat = threat.NeedsBonusExternalThreat ?
				new InternalThreatModel(((IThreatWithBonusThreat<InternalThreat>)threat).BonusThreat) :
				null;
			BonusExternalThreat = threat.NeedsBonusExternalThreat ?
				new ExternalThreatModel(((IThreatWithBonusThreat<ExternalThreat>)threat).BonusThreat) :
				null;
		}

		[JsonConstructor]
		protected ThreatModel()
		{
		}
	}
}
