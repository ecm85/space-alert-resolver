using BLL.Players;

namespace BLL.ShipComponents
{
    public abstract class HeavyLaserCannon : LaserCannon
    {
        protected HeavyLaserCannon(Reactor source, int baseDamage, ZoneLocation currentZone)
            : base(source, baseDamage, PlayerDamageType.HeavyLaser, currentZone)
        {
        }
    }
}
