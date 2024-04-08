using System.Text.Json.Serialization;

namespace BLL.Threats
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ThreatDifficulty
    {
        White,
        Yellow,
        Red
    }
}
