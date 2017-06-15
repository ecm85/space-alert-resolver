namespace BLL.ShipComponents
{
    public class SideHeavyLaserCannon : HeavyLaserCannon
    {
        internal SideHeavyLaserCannon(Reactor source, ZoneLocation currentZone) : base(source, 4, currentZone)
        {
        }
    }
}
