using BLL.Threats;

namespace PL.Models
{
	public class ThreatSnapshotModel
	{
		public ThreatType ThreatType { get; set; }
		public ThreatDifficulty ThreatDifficulty { get; set; }
		public int Position { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; set; }
		public string Description { get; }

		public ThreatSnapshotModel(Threat threat)
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
