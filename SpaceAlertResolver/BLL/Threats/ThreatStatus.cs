using System.Text.Json.Serialization;

namespace BLL.Threats
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ThreatStatus
	{
		NotAppeared,
		OnShip,
		OnTrack,
		PhasedOut,
		Survived,
		Defeated,
		Cryoshielded,
		KnockedOffCourse,
		Stealthed,
		GrownUp,
		BonusShield,
		BonusAttack,
		ReducedMovement
	}

	public static class ThreatStatusExtensions
	{
		public static bool IsBuff(this ThreatStatus threatStatus)
		{
			switch (threatStatus)
			{
				case ThreatStatus.Cryoshielded:
				case ThreatStatus.Stealthed:
				case ThreatStatus.GrownUp:
					return true;
			}
			return false;
		}

		public static bool IsDebuff(this ThreatStatus threatStatus)
		{
			switch (threatStatus)
			{
				case ThreatStatus.KnockedOffCourse:
					return true;
			}
			return false;
		}
	}
}
