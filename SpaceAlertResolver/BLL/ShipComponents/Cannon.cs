using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class Cannon
	{
		private readonly int damage;
		private readonly IList<ZoneLocation> zonesAffected;
		private readonly int range;
		private readonly DamageType damageType;
		private readonly EnergyContainer source;


		public PlayerDamage PerformAAction()
		{
			if (source.Energy > 1)
			{
				source.Energy -= 1;
				return new PlayerDamage(damage, damageType, range, zonesAffected);
			}
			return null;
		}

		protected Cannon(EnergyContainer source, int damage, int range, DamageType damageType, ZoneLocation zoneAffected)
			: this(source, damage, range, damageType, new[] { zoneAffected })
		{
		}

		protected Cannon(EnergyContainer source, int damage, int range, DamageType damageType, IList<ZoneLocation> zonesAffected)
		{
			this.source = source;
			this.damage = damage;
			this.range = range;
			this.damageType = damageType;
			this.zonesAffected = zonesAffected;
		}
	}
}
