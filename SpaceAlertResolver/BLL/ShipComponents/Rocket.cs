using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class Rocket
	{
		public PlayerDamage PerformAttack()
		{
			return new PlayerDamage(3, DamageType.Rocket, 2, EnumFactory.All<ZoneLocation>());
		}
	}
}
