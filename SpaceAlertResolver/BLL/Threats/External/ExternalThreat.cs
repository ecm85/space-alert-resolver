using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Common;
using BLL.ShipComponents;

namespace BLL.Threats.External
{
	public abstract class ExternalThreat : Threat
	{
		public ZoneLocation CurrentZone { get; set; }
		protected int Shields { get; set; }

		protected int DistanceToShip { get { return Track.DistanceToThreat(Position.GetValueOrDefault()); } }

		protected ExternalThreat(ThreatType type, ThreatDifficulty difficulty, int shields, int health, int speed) :
			base(type, difficulty, health, speed)
		{
			Shields = shields;
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
			var damageSum = damages.Sum(damage => damage.Amount);
			TakeDamage(damageSum, maxDamageTaken);
		}

		protected void TakeDamage(int damageSum, int? maxDamageTaken)
		{
			var bonusShields = ThreatController.CurrentExternalThreatBuffs().Count(buff => buff == ExternalThreatEffect.BonusShield);
			var damageDealt = damageSum - (Shields + bonusShields);
			if (damageDealt > 0)
				RemainingHealth -= maxDamageTaken.HasValue ? Math.Min(damageDealt, maxDamageTaken.Value) : damageDealt;
			CheckDefeated();
		}

		public virtual bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			var isInRange = damage.AffectedDistances.Contains(DistanceToShip);
			var gunCanHitCurrentZone = damage.AffectedZones.Contains(CurrentZone);
			return IsDamageable && isInRange && gunCanHitCurrentZone;
		}

		public virtual bool IsPriorityTargetFor(PlayerDamage damage)
		{
			return false;
		}

		protected void AttackSpecificZones(int amount, IList<ZoneLocation> zones)
		{
			AttackSpecificZones(amount, zones, ThreatDamageType.Standard);
		}

		private void AttackSpecificZones(int amount, IList<ZoneLocation> zones, ThreatDamageType threatDamageType)
		{
			Attack(amount, threatDamageType, zones);
		}

		protected ThreatDamageResult AttackCurrentZone(int amount)
		{
			return AttackCurrentZone(amount, ThreatDamageType.Standard);
		}

		protected ThreatDamageResult AttackCurrentZone(int amount, ThreatDamageType threatDamageType)
		{
			return Attack(amount, threatDamageType, new[] { CurrentZone });
		}

		protected void AttackAllZones(int amount)
		{
			AttackAllZones(amount, ThreatDamageType.Standard);
		}

		protected void AttackAllZones(int amount, ThreatDamageType threatDamageType)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>());
		}

		protected void AttackOtherTwoZones(int amount)
		{
			AttackOtherTwoZones(amount, ThreatDamageType.Standard);
		}

		protected void AttackOtherTwoZones(int amount, ThreatDamageType threatDamageType)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>().Except(new[] { CurrentZone }).ToList());
		}

		private ThreatDamageResult Attack(int amount, ThreatDamageType threatDamageType, IList<ZoneLocation> zoneLocations)
		{
			var bonusAttacks = ThreatController.CurrentExternalThreatBuffs().Count(buff => buff == ExternalThreatEffect.BonusAttack);
			var damage = new ThreatDamage(amount + bonusAttacks, threatDamageType, zoneLocations, DistanceToShip);
			var result = SittingDuck.TakeAttack(damage);
			if (result.ShipDestroyed)
				throw new LoseException(this);
			return result;
		}
	}
}
