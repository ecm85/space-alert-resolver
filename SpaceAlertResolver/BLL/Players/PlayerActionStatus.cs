using System.Text.Json.Serialization;

namespace BLL.Players
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum PlayerActionStatus
	{
		NotPerformed,
		Performing,
		Performed
	}
}
