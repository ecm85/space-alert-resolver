using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class LightLaserCannon : LaserCannon
	{
		protected LightLaserCannon(BatteryPack source, ZoneLocation currentZone)
			: base(source, 2, DamageType.LightLaser, currentZone)
		{
		}
	}
}
