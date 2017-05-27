using System;
using System.Collections.Generic;
using BLL.Players;

namespace BLL.ShipComponents
{
	public abstract class Cannon : IAlphaComponent
	{
		private readonly int baseDamage;
		private readonly IList<ZoneLocation> affectedZones;
		private readonly IEnumerable<int> baseAffectedDistances;
		private readonly PlayerDamageType playerDamageType;
		private readonly IEnergyProvider source;
		protected bool OpticsDisrupted { get; private set; }
		protected int BaseDamage { get { return baseDamage;} }
		protected IList<ZoneLocation> AffectedZones { get { return affectedZones;} }
		protected IEnumerable<int> BaseAffectedDistances { get { return baseAffectedDistances;} }
		protected PlayerDamageType PlayerDamageType { get { return playerDamageType;} }
		public EnergyType? EnergyInCannon { get; private set; }

		public void SetOpticsDisrupted(bool opticsDisrupted)
		{
			OpticsDisrupted = opticsDisrupted;
		}

		public event EventHandler CannonFired = (sender, args) => { };
		public IEnumerable<PlayerDamage> CurrentPlayerDamage { get; private set; }

		public void PerformEndOfTurn()
		{
			CurrentPlayerDamage = null;
			EnergyInCannon = null;
			source.PerformEndOfTurn();
		}

		public void PerformAAction(bool isHeroic, Player performingPlayer, bool isAdvanced)
		{
			if (CanFire())
			{
				source.UseEnergy(1);
				EnergyInCannon = source.EnergyType;
				CannonFired(this, EventArgs.Empty);
				CurrentPlayerDamage = GetPlayerDamage(performingPlayer, isHeroic, isAdvanced);
			}
			HasMechanicBuff = false;
		}

		public bool CanFire()
		{
			return CurrentPlayerDamage == null && source.CanUseEnergy(1);
		}

		protected abstract IEnumerable<PlayerDamage> GetPlayerDamage(Player performingPlayer, bool isHeroic, bool isAdvanced);

		protected bool IsDamaged { get; private set; }

		public void SetDamaged(bool isCampaignDamage)
		{
			IsDamaged = true;
		}

		public void Repair()
		{
			IsDamaged = false;
		}

		protected Cannon(IEnergyProvider source, int baseDamage, IEnumerable<int> baseAffectedDistances, PlayerDamageType playerDamageType, params ZoneLocation[] affectedZones)
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
