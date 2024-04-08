using System.Text.Json.Serialization;

namespace BLL
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ZoneDebuff
    {
        DoubleDamage,
        DisruptedOptics,
        IneffectiveShields,
        ReversedShields
    }
}
