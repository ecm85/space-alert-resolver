using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class Rocket
	{
		private bool IsDoubleRocket { get; set; }

		public void SetDoubleRocket()
		{
			IsDoubleRocket = true;
		}

		public PlayerDamage PerformAttack(Player performingPlayer)
		{
			return new PlayerDamage(IsDoubleRocket ? 5 : 3, PlayerDamageType.Rocket, new [] {1, 2}, EnumFactory.All<ZoneLocation>(), performingPlayer);
		}
	}
}
