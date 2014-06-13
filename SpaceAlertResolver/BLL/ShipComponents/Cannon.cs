using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class Cannon
	{
		protected int damage;
		protected readonly IList<ZoneLocation> zonesAffected;
		protected int range;
		protected readonly PlayerDamageType playerDamageType;
		private readonly EnergyContainer source;
		public bool DisruptedOptics { get; set; }

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
				var amount = isHeroic ? damage + 1 : damage;
				return GetPlayerDamage(amount);
			}
			return null;
		}

		protected virtual PlayerDamage GetPlayerDamage(int amount)
		{
			return new PlayerDamage(amount, playerDamageType, range, zonesAffected);
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
