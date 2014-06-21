using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.External
{
	public abstract class ExternalThreat : Threat
	{
		public ZoneLocation CurrentZone { get; set; }
		protected int shields;

		protected override void PlaceOnTrack(Track track, int trackPosition)
		{
			ThreatController.ExternalThreatsMove += PerformMove;
			base.PlaceOnTrack(track, trackPosition);
		}

		protected int DistanceToShip { get { return Track.DistanceToThreat(Position.GetValueOrDefault()); } }

		protected ExternalThreat(ThreatType type, ThreatDifficulty difficulty, int shields, int health, int speed) :
			base(type, difficulty, health, speed)
		{
			this.shields = shields;
		}

		public void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears, ZoneLocation currentZone)
		{
			SittingDuck = sittingDuck;
			ThreatController = threatController;
			TimeAppears = timeAppears;
			CurrentZone = currentZone;
		}

		public void TakeIrreducibleDamage(int amount)
		{
			RemainingHealth -= amount;
			CheckDefeated();
		}

		public virtual void TakeDamage(IList<PlayerDamage> damages)
		{
			TakeDamage(damages, null);
		}

		protected void TakeDamage(IEnumerable<PlayerDamage> damages, int? maxDamageTaken)
		{
			var bonusShields = ThreatController.CurrentExternalThreatBuffs().Count(buff => buff == ExternalThreatBuff.BonusShield);
			var damageDealt = damages.Sum(damage => damage.Amount) - (shields + bonusShields);
			if (damageDealt > 0)
				RemainingHealth -= maxDamageTaken.HasValue ? Math.Min(damageDealt, maxDamageTaken.Value) : damageDealt;
			CheckDefeated();
		}

		public virtual bool CanBeTargetedBy(PlayerDamage damage)
		{
			var isInRange = damage.AffectedDistance.Contains(DistanceToShip);
			var gunCanHitCurrentZone = damage.ZoneLocations.Contains(CurrentZone);
			return IsDamageable && isInRange && gunCanHitCurrentZone;
		}

		public virtual bool IsPriorityTargetFor(PlayerDamage damage)
		{
			return false;
		}

		protected void AttackSpecificZones(int amount, IList<ZoneLocation> zones, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, zones);
		}

		protected ThreatDamageResult Attack(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			return Attack(amount, threatDamageType, new[] { CurrentZone });
		}

		protected void AttackAllZones(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>());
		}

		protected void AttackOtherTwoZones(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>().Except(new[] { CurrentZone }).ToList());
		}

		private ThreatDamageResult Attack(int amount, ThreatDamageType threatDamageType, IList<ZoneLocation> zoneLocations)
		{
			var bonusAttacks = ThreatController.CurrentExternalThreatBuffs().Count(buff => buff == ExternalThreatBuff.BonusAttack);
			var damage = new ThreatDamage(amount + bonusAttacks, threatDamageType, zoneLocations, DistanceToShip);
			var result = SittingDuck.TakeAttack(damage);
			if (result.ShipDestroyed)
				throw new LoseException(this);
			return result;
		}

		protected override void OnThreatTerminated()
		{
			ThreatController.ExternalThreatsMove -= PerformMove;
			base.OnThreatTerminated();
		}
	}
}
