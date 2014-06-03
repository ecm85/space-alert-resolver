using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
	public class PlayerInterceptorDamage
	{
		private readonly bool isHeroic;
		public PlayerInterceptorDamage(bool isHeroic)
		{
			this.isHeroic = isHeroic;
		}

		public PlayerDamage SingleDamage
		{
			get { return new PlayerDamage(isHeroic ? 4 : 3, PlayerDamageType.InterceptorsSingle, 1, EnumFactory.All<ZoneLocation>()); }
		}

		public PlayerDamage MultipleDamage
		{
			get {return new PlayerDamage(isHeroic ? 2 : 1, PlayerDamageType.InterceptorsMultiple, 1, EnumFactory.All<ZoneLocation>());}
		}
	}
}
