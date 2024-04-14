using System.Text.Json.Serialization;

namespace BLL
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ThreatDamageType
	{
		Standard,
		IgnoresShields,
		Plasmatic,
		DoubleDamageThroughShields,
		ReducedByTwoAgainstInterceptors
	}
}
