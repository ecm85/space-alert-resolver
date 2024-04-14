using System.Text.Json.Serialization;

namespace BLL
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum DamageTargetType
	{
		Single,
		All
	}
}
