using BLL;
using BLL.ShipComponents;

namespace PL.Models
{
    public class InitialDamageModel
    {
        public ZoneLocation ZoneLocation { get; set; }
        public DamageToken DamageToken { get; set; }
    }
}