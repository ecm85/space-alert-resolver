using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class LaserCannon : Cannon
	{
		protected LaserCannon(EnergyContainer source, int damage, DamageType damageType, ZoneLocation currentZone)
			: base(source, damage, 3, damageType, currentZone)
		{
		}
	}
}
