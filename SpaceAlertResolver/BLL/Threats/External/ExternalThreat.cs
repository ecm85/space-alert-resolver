using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.External
{
	public abstract class ExternalThreat : Threat
	{
		public Zone CurrentZone { get; private set; }
		protected int shields;
		private ExternalTrack Track { get; set; }

		public void SetTrack(ExternalTrack track)
		{
			Track = track;
		}

		private int DistanceToShip { get { return Track.DistanceToThreat(this); } }
		public int TrackPosition  { get { return Track.ThreatPositions[this]; }}

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

		protected virtual ExternalThreatDamageResult Attack(int amount)
		{
			return AttackZone(amount, CurrentZone);
		}

		protected void AttackAllZones(int amount)
		{
			AttackZones(amount, sittingDuck.Zones);
		}

		protected void AttackOtherTwoZones(int amount)
		{
			AttackZones(amount, sittingDuck.Zones.Except(new[] { CurrentZone }));
		}

		private void AttackZones(int amount, IEnumerable<Zone> zones)
		{
			foreach (var zone in zones)
				AttackZone(amount, zone);
		}

		private ExternalThreatDamageResult AttackZone(int amount, Zone zone)
		{
			var result = zone.TakeAttack(amount);
			if (result.ShipDestroyed)
				throw new LoseException(this);
			return result;
		}
	}
}
