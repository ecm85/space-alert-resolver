using BLL.Players;

namespace BLL.ShipComponents
{
	public abstract class LightLaserCannon : LaserCannon
	{
		protected LightLaserCannon(BatteryPack source, ZoneLocation currentZone)
			: base(source, 2, PlayerDamageType.LightLaser, currentZone)
		{
		}
	}
}
