using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class PlayerInterceptorDamage
	{
		public PlayerDamage SingleDamage
		{
			get { return new PlayerDamage(3, DamageType.InterceptorsSingle, 1, EnumFactory.All<ZoneLocation>()); }
		}

		public PlayerDamage MultipleDamage
		{
			get {return new PlayerDamage(1, DamageType.InterceptorsMultiple, 1, EnumFactory.All<ZoneLocation>());}
		}
	}
}
