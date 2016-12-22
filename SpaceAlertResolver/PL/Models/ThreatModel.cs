using BLL.Threats;
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
		public int Position { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; set; }
		public string Description { get; }

		public ThreatModel(Threat threat)
		{
			Position = threat.Position.GetValueOrDefault();
			RemainingHealth = threat.RemainingHealth;
			Speed = threat.Speed;
			ThreatType = threat.ThreatType;
			ThreatDifficulty = threat.Difficulty;
			Description = threat.GetType().Name;
		}
	}
}
