using BLL.ShipComponents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
    public class CannonModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EnergyType? Energy { get; set; }

        public CannonModel(IAlphaComponent cannon)
        {
            Energy = cannon.EnergyInCannon;
        }
    }
}
