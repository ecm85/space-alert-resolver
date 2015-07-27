using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class Cannon : IDamageableComponent
	{
		protected readonly int BaseDamage;
		protected readonly IList<ZoneLocation> AffectedZones;
		protected readonly int[] BaseAffectedDistances;
		protected readonly PlayerDamageType PlayerDamageType;
		private readonly EnergyContainer source;
		protected bool OpticsDisrupted { get; private set; }

		public void SetOpticsDisrupted(bool opticsDisrupted)
		{
			OpticsDisrupted = opticsDisrupted;
		}

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
			MechanicBuff = false;
		}

		public bool CanFire()
		{
			return PlayerDamage == null && source.Energy > 1;
		}

		protected abstract PlayerDamage[] GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced);

		protected bool IsDamaged;

		public void SetDamaged()
		{
			IsDamaged = true;
		}

		public void Repair()
		{
			IsDamaged = false;
		}

		protected Cannon(EnergyContainer source, int baseDamage, int[] baseAffectedDistances, PlayerDamageType playerDamageType, ZoneLocation zoneAffected)
			: this(source, baseDamage, baseAffectedDistances, playerDamageType, new[] { zoneAffected })
		{
		}

		protected Cannon(EnergyContainer source, int baseDamage, int[] baseAffectedDistances, PlayerDamageType playerDamageType, IList<ZoneLocation> affectedZones)
		{
			this.source = source;
			BaseDamage = baseDamage;
			BaseAffectedDistances = baseAffectedDistances;
			PlayerDamageType = playerDamageType;
			AffectedZones = affectedZones;
		}

		protected bool MechanicBuff;

		public void RemoveMechanicBuff()
		{
			MechanicBuff = false;
		}

		public void AddMechanicBuff()
		{
			MechanicBuff = true;
		}
	}
}
