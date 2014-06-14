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
		public ZoneLocation CurrentZone { get; private set; }
		protected int shields;

		protected override void PlaceOnTrack(Track track, int trackPosition)
		{
			ThreatController.ExternalThreatsMove += PerformMove;
			base.PlaceOnTrack(track, trackPosition);
		}

		private int DistanceToShip { get { return Track.DistanceToThreat(Position.GetValueOrDefault()); } }

		protected ExternalThreat(ThreatType type, ThreatDifficulty difficulty, int shields, int health, int speed) :
			base(type, difficulty, health, speed)
		{
			this.shields = shields;
		}

		public virtual void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears, ZoneLocation currentZone)
		{
			SittingDuck = sittingDuck;
			ThreatController = threatController;
			TimeAppears = timeAppears;
			CurrentZone = currentZone;
		}

		public virtual bool IsDamageable { get { return HasBeenPlaced && Position != null; }}

		public virtual void TakeIrreducibleDamage(int amount)
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
			var bonusShields = SittingDuck.CurrentExternalThreatBuffs().Count(buff => buff == ExternalThreatBuff.BonusShield);
			var damageDealt = damages.Sum(damage => damage.Amount) - (shields + bonusShields);
			if (damageDealt > 0)
				RemainingHealth -= maxDamageTaken.HasValue ? Math.Min(damageDealt, maxDamageTaken.Value) : damageDealt;
			CheckDefeated();
		}

		public virtual bool CanBeTargetedBy(PlayerDamage damage)
		{
			var isInRange = damage.Range >= DistanceToShip;
			var gunCanHitCurrentZone = damage.ZoneLocations.Contains(CurrentZone);
			return isInRange && gunCanHitCurrentZone;
		}

		public virtual bool IsPriorityTargetFor(PlayerDamage damage)
		{
			return false;
		}

		protected void AttackSpecificZones(int amount, IList<ZoneLocation> zones, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, zones);
		}

		protected void Attack(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, new [] {CurrentZone});
		}

		protected void AttackAllZones(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>());
		}

		protected void AttackOtherTwoZones(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>().Except(new[] { CurrentZone }).ToList());
		}

		private void Attack(int amount, ThreatDamageType threatDamageType, IList<ZoneLocation> zoneLocations)
		{
			var bonusAttacks = SittingDuck.CurrentExternalThreatBuffs().Count(buff => buff == ExternalThreatBuff.BonusAttack);
			var damage = new ThreatDamage(amount + bonusAttacks, threatDamageType, zoneLocations);
			var result = SittingDuck.TakeAttack(damage);
			if (result.ShipDestroyed)
				throw new LoseException(this);
		}

		protected override void OnHealthReducedToZero()
		{
			ThreatController.ExternalThreatsMove -= PerformMove;
			base.OnHealthReducedToZero();
		}

		protected override void OnReachingEndOfTrack()
		{
			ThreatController.ExternalThreatsMove -= PerformMove;
			base.OnReachingEndOfTrack();
		}
	}
}
