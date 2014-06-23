using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class LaserCannon : Cannon
	{
		protected LaserCannon(EnergyContainer source, int baseDamage, PlayerDamageType playerDamageType, ZoneLocation currentZone)
			: base(source, baseDamage, new [] {1, 2, 3}, playerDamageType, currentZone)
		{
		}

		protected override PlayerDamage[] GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced)
		{
			var damage = baseDamage;
			if (isHeroic)
				damage++;
			if (isDamaged)
				damage--;
			if (mechanicBuff)
				damage++;
			return new [] {new PlayerDamage(damage, playerDamageType, baseAffectedDistances, affectedZones, performingPlayer, DisruptedOptics)};
		}
	}
}
