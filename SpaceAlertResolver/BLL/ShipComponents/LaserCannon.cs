using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class LaserCannon : Cannon
	{
		protected LaserCannon(EnergyContainer source, int damage, PlayerDamageType playerDamageType, ZoneLocation currentZone)
			: base(source, damage, new [] {1, 2, 3}, playerDamageType, currentZone)
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

		protected override PlayerDamage[] GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced)
		{
			var amount = isHeroic ? damage + 1 : damage;
			return new [] {new PlayerDamage(amount, playerDamageType, distancesAffected, zonesAffected, performingPlayer, DisruptedOptics)};
		}
	}
}
