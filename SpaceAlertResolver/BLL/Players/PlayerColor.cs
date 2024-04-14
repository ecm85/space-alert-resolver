using System.Text.Json.Serialization;

namespace BLL.Players
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum PlayerColor
	{
		Blue,
		Green,
		Red,
		Yellow,
		Purple
	}
}
