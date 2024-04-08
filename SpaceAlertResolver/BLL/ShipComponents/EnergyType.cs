using System.Text.Json.Serialization;

namespace BLL.ShipComponents
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnergyType
    {
        Standard,
        Battery
    }
}
