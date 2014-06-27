using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class Cannon : IDamageableComponent
	{
		protected int baseDamage;
		protected readonly IList<ZoneLocation> affectedZones;
		protected int[] baseAffectedDistances;
		protected readonly PlayerDamageType playerDamageType;
		private readonly EnergyContainer source;
		public bool DisruptedOptics { get; set; }
		public event Action CannonFired = () => { };
		public PlayerDamage[] PlayerDamage { get; private set; }

		public void PerformEndOfTurn()
		{
			PlayerDamage = null;
		}

		public void PerformAAction(bool isHeroic, Player performingPlayer, bool isAdvanced = false)
		{
			if (CanFire())
			{
				source.Energy -= 1;
				CannonFired();
				PlayerDamage = GetPlayerDamage(performingPlayer, isHeroic, isAdvanced);
			}
			mechanicBuff = false;
		}

		public bool CanFire()
		{
			return PlayerDamage == null && source.Energy > 1;
		}

		protected abstract PlayerDamage[] GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced);

		protected bool isDamaged;

		public void SetDamaged()
		{
			isDamaged = true;
		}

		public void Repair()
		{
			isDamaged = false;
		}

		protected Cannon(EnergyContainer source, int baseDamage, int[] baseAffectedDistances, PlayerDamageType playerDamageType, ZoneLocation zoneAffected)
			: this(source, baseDamage, baseAffectedDistances, playerDamageType, new[] { zoneAffected })
		{
		}

		protected Cannon(EnergyContainer source, int baseDamage, int[] baseAffectedDistances, PlayerDamageType playerDamageType, IList<ZoneLocation> affectedZones)
		{
			this.source = source;
			this.baseDamage = baseDamage;
			this.baseAffectedDistances = baseAffectedDistances;
			this.playerDamageType = playerDamageType;
			this.affectedZones = affectedZones;
		}

		protected bool mechanicBuff;

		public void RemoveMechanicBuff()
		{
			mechanicBuff = false;
		}

		public void AddMechanicBuff()
		{
			mechanicBuff = true;
		}
	}
}
