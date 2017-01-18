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
		public int TimeAppears { get; set; }
		public string Id { get; set; }
		public string DisplayName { get; set; }
		public string FileName { get; set; }
		public int Points { get; set; }

		public ThreatModel(Threat threat)
		{
			Position = threat.Position.GetValueOrDefault();
			RemainingHealth = threat.RemainingHealth;
			Speed = threat.Speed;
			ThreatType = threat.ThreatType;
			ThreatDifficulty = threat.Difficulty;
			TimeAppears = threat.TimeAppears;
			Id = threat.Id;
			DisplayName = threat.DisplayName;
			FileName = threat.FileName;
			Points = threat.Points;
		}

		[JsonConstructor]
		protected ThreatModel()
		{
		}
	}
}
