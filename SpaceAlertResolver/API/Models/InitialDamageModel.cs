using BLL;
using BLL.ShipComponents;

namespace API.Models
{
    public class InitialDamageModel
    {
        public ZoneLocation ZoneLocation { get; set; }
        public DamageToken DamageToken { get; set; }
    }
}