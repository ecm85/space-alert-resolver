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
			var damage = BaseDamage;
			if (isHeroic)
				damage++;
			if (IsDamaged)
				damage--;
			if (MechanicBuff)
				damage++;
			return new [] {new PlayerDamage(damage, PlayerDamageType, BaseAffectedDistances, AffectedZones, performingPlayer, OpticsDisrupted)};
		}
	}
}
