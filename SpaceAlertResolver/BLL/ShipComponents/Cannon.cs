using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class Cannon : IDamageableComponent
	{
		private readonly int baseDamage;
		private readonly IList<ZoneLocation> affectedZones;
		private readonly IEnumerable<int> baseAffectedDistances;
		private readonly PlayerDamageType playerDamageType;
		private readonly EnergyContainer source;
		protected bool OpticsDisrupted { get; private set; }
		protected int BaseDamage { get { return baseDamage;} }
		protected IList<ZoneLocation> AffectedZones { get { return affectedZones;} }
		protected IEnumerable<int> BaseAffectedDistances { get { return baseAffectedDistances;} }
		protected PlayerDamageType PlayerDamageType { get { return playerDamageType;} }

		public void SetOpticsDisrupted(bool opticsDisrupted)
		{
			OpticsDisrupted = opticsDisrupted;
		}

		public event EventHandler CannonFired = (sender, args) => { };
		public IEnumerable<PlayerDamage> CurrentPlayerDamage { get; private set; }

		public void PerformEndOfTurn()
		{
			CurrentPlayerDamage = null;
		}

		public void PerformAAction(bool isHeroic, Player performingPlayer, bool isAdvanced)
		{
			if (CanFire())
			{
				source.Energy -= 1;
				CannonFired(this, EventArgs.Empty);
				CurrentPlayerDamage = GetPlayerDamage(performingPlayer, isHeroic, isAdvanced);
			}
			HasMechanicBuff = false;
		}

		public bool CanFire()
		{
			return CurrentPlayerDamage == null && source.Energy > 1;
		}

		protected abstract IEnumerable<PlayerDamage> GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced);

		protected bool IsDamaged { get; private set; }

		public void SetDamaged()
		{
			IsDamaged = true;
		}

		public void Repair()
		{
			IsDamaged = false;
		}

		protected Cannon(EnergyContainer source, int baseDamage, IEnumerable<int> baseAffectedDistances, PlayerDamageType playerDamageType, params ZoneLocation[] affectedZones)
		{
			this.source = source;
			this.baseDamage = baseDamage;
			this.baseAffectedDistances = baseAffectedDistances;
			this.playerDamageType = playerDamageType;
			this.affectedZones = affectedZones;
		}

		protected bool HasMechanicBuff { get; private set; }

		public void RemoveMechanicBuff()
		{
			HasMechanicBuff = false;
		}

		public void AddMechanicBuff()
		{
			HasMechanicBuff = true;
		}
	}
}
