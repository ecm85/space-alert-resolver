using BLL.Threats;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public abstract class ThreatModel
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public ThreatType ThreatType { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public ThreatDifficulty ThreatDifficulty { get; set; }
		public int Position { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; set; }
		public string Description { get; }
		public int TimeAppears { get; }
		public string Id { get; }

		protected ThreatModel(Threat threat)
		{
			Position = threat.Position.GetValueOrDefault();
			RemainingHealth = threat.RemainingHealth;
			Speed = threat.Speed;
			ThreatType = threat.ThreatType;
			ThreatDifficulty = threat.Difficulty;
			Description = threat.GetType().Name;
			TimeAppears = threat.TimeAppears;
			Id = ThreatFactory.ThreatIdsByType[threat.GetType()];
		}
	}
}
