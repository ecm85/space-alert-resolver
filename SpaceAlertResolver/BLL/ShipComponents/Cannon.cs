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
		protected int[] distancesAffected;
		protected readonly PlayerDamageType playerDamageType;
		private readonly EnergyContainer source;
		public bool DisruptedOptics { get; set; }

		private bool firedThisTurn;

		public void PerformEndOfTurn()
		{
			firedThisTurn = false;
		}

		public PlayerDamage PerformAAction(bool isHeroic, Player performingPlayer)
		{
			if (!firedThisTurn && source.Energy > 1)
			{
				firedThisTurn = true;
				source.Energy -= 1;
				var amount = isHeroic ? damage + 1 : damage;
				return GetPlayerDamage(amount, performingPlayer);
			}
			return null;
		}

		protected virtual PlayerDamage GetPlayerDamage(int amount, Player performingPlayer)
		{
			return new PlayerDamage(amount, playerDamageType, distancesAffected, zonesAffected, performingPlayer);
		}

		protected bool IsDamaged { get; set; }

		public abstract void SetDamaged();
		public abstract void Repair();

		protected Cannon(EnergyContainer source, int damage, int[] distancesAffected, PlayerDamageType playerDamageType, ZoneLocation zoneAffected)
			: this(source, damage, distancesAffected, playerDamageType, new[] { zoneAffected })
		{
		}

		protected Cannon(EnergyContainer source, int damage, int[] distancesAffected, PlayerDamageType playerDamageType, IList<ZoneLocation> zonesAffected)
		{
			this.source = source;
			this.damage = damage;
			this.distancesAffected = distancesAffected;
			this.playerDamageType = playerDamageType;
			this.zonesAffected = zonesAffected;
		}
	}
}
