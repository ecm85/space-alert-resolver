using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.External
{
	public abstract class ExternalThreat : Threat
	{
		public Zone CurrentZone { get; protected set; }
		protected int shields;
		public ExternalTrack Track { get; set; }

		private int DistanceToShip { get { return Track.DistanceToThreat(this); } }
		public int TrackPosition  { get { return Track.threatPositions[this]; }}

		protected ExternalThreat(ThreatType type, ThreatDifficulty difficulty, int shields, int health, int speed, int timeAppears, Zone currentZone, SittingDuck sittingDuck) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			this.shields = shields;
			CurrentZone = currentZone;
		}

		public virtual void TakeDamage(IList<PlayerDamage> damages)
		{
			var damageDealt = damages.Sum(damage => damage.Amount) - shields;
			if (damageDealt > 0)
				RemainingHealth -= damageDealt;
			CheckForDestroyed();
		}

		public virtual bool CanBeTargetedBy(PlayerDamage damage)
		{
			var isInRange = damage.Range >= DistanceToShip;
			var gunCanHitCurrentZone = damage.ZoneLocations.Contains(CurrentZone.ZoneLocation);
			return isInRange && gunCanHitCurrentZone;
		}

		protected virtual ExternalPlayerDamageResult Attack(int amount)
		{
			return CurrentZone.TakeAttack(amount);
		}

		protected virtual ExternalPlayerDamageResult AttackAllZones(int amount)
		{
			return AttackZones(amount, sittingDuck.Zones);
		}

		protected virtual ExternalPlayerDamageResult AttackOtherTwoZones(int amount)
		{
			return AttackZones(amount, sittingDuck.Zones.Except(new[] { CurrentZone }));
		}

		private ExternalPlayerDamageResult AttackZones(int amount, IEnumerable<Zone> zones)
		{
			var result = new ExternalPlayerDamageResult();
			foreach (var zone in zones)
				result.AddDamage(zone.TakeAttack(amount));
			return result;
		}
	}
}
