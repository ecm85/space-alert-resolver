using System.Text.Json.Serialization;

namespace BLL
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DamageToken
    {
        Reactor,
        Shield,
        FrontCannon,
        BackCannon,
        Gravolift,
        Structural
    }
}
