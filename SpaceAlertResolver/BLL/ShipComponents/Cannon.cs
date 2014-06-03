using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class Cannon
	{
		protected int damage;
		private readonly IList<ZoneLocation> zonesAffected;
		protected int range;
		private readonly PlayerDamageType playerDamageType;
		private readonly EnergyContainer source;

		private bool firedThisTurn;

		public void PerformEndOfTurn()
		{
			firedThisTurn = false;
		}

		public PlayerDamage PerformAAction(bool isHeroic)
		{
			if (!firedThisTurn && source.Energy > 1)
			{
				firedThisTurn = true;
				source.Energy -= 1;
				return new PlayerDamage(isHeroic ? damage + 1 : damage, playerDamageType, range, zonesAffected);
			}
			return null;
		}

		protected bool IsDamaged { get; set; }

		public abstract void SetDamaged();
		public abstract void Repair();

		protected Cannon(EnergyContainer source, int damage, int range, PlayerDamageType playerDamageType, ZoneLocation zoneAffected)
			: this(source, damage, range, playerDamageType, new[] { zoneAffected })
		{
		}

		protected Cannon(EnergyContainer source, int damage, int range, PlayerDamageType playerDamageType, IList<ZoneLocation> zonesAffected)
		{
			this.source = source;
			this.damage = damage;
			this.range = range;
			this.playerDamageType = playerDamageType;
			this.zonesAffected = zonesAffected;
		}
	}
}
