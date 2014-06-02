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

		public override void SetDamaged()
		{
			var wasAlreadyDamaged = IsDamaged;
			IsDamaged = true;
			if (!wasAlreadyDamaged)
				damage -= 1;
		}

		public override void Repair()
		{
			var wasAlreadyDamaged = IsDamaged;
			IsDamaged = false;
			if (wasAlreadyDamaged)
				damage += 1;
		}
	}
}
