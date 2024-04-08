using System.Text.Json.Serialization;
using BLL.ShipComponents;

namespace API.Models
{
    public class CannonModel
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnergyType? Energy { get; set; }

        public CannonModel(IAlphaComponent cannon)
        {
            Energy = cannon.EnergyInCannon;
        }
    }
}
