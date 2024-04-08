using System.Text.Json.Serialization;

namespace BLL.Players
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlayerSpecialization
    {
        Rocketeer,
        DataAnalyst,
        EnergyTechnician,
        PulseGunner,
        Medic,
        Teleporter,
        Hypernavigator,
        SpecialOps,
        SquadLeader,
        Mechanic
    }
}
