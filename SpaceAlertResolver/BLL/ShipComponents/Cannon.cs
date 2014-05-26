using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class Cannon
	{
		private readonly int damage;
		private readonly ZoneType[] zoneTypesAffected;
		private readonly int range;
		private readonly DamageType damageType;
		private readonly EnergyContainer source;


		public Damage PerformAAction()
		{
			if (source.Energy > 1)
			{
				source.Energy -= 1;
				return new Damage
				{
					Amount = damage,
					DamageType = damageType,
					Range = range,
					ZoneTypesAffected = zoneTypesAffected
				};
			}
			return null;
		}

		protected Cannon(EnergyContainer source, int damage, int range, DamageType damageType, params ZoneType[] zoneTypesAffected)
		{
			this.source = source;
			this.damage = damage;
			this.range = range;
			this.damageType = damageType;
			this.zoneTypesAffected = zoneTypesAffected;
		}
	}
}
