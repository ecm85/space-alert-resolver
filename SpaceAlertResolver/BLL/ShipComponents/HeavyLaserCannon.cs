using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
