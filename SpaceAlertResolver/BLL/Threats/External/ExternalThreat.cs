using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.External
{
	public abstract class ExternalThreat : Threat
	{
		public ZoneLocation CurrentZone { get; private set; }
		protected int shields;
		private ExternalTrack Track { get; set; }

		public void PlaceOnTrack(ExternalTrack track)
		{
			PlaceOnTrack(track, track.GetStartingPosition());
		}

		private void PlaceOnTrack(ExternalTrack track, int? trackPosition)
		{
			Track = track;
			Position = trackPosition;
			HasBeenPlaced = true;
		}

		private int DistanceToShip { get { return Track.DistanceToThreat(this); } }

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
			if (!IsOnTrack())
				return false;
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

		public virtual void PerformEndOfDamageResolution()
		{
		}

		public override void Move(int currentTurn)
		{
			Move(Speed, currentTurn);
		}

		public void Move(int amount, int currentTurn)
		{
			if (!IsOnTrack())
				return;
			BeforeMove();
			Track.MoveThreat(this, amount, currentTurn);
			AfterMove();
		}
	}
}
