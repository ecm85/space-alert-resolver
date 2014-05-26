using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class LightLaserCannon : LaserCannon
	{
		protected LightLaserCannon(BatteryPack source, ZoneType currentZoneType)
			: base(source, 2, DamageType.LightLaser, currentZoneType)
		{
		}
	}
}
