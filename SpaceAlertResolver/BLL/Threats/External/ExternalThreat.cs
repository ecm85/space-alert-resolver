using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public abstract class ExternalThreat : Threat
	{
		public Zone CurrentZone { get; protected set; }
		protected int shields;

		protected ExternalThreat(int pointsForSurviving, int pointsForDefeating, int shields, int health, int speed, int timeAppears, Zone currentZone, SittingDuck sittingDuck) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears, sittingDuck)
		{
			this.shields = shields;
			CurrentZone = currentZone;
		}

		public virtual void TakeDamage(IList<PlayerDamage> damages)
		{
			var damageDealt = damages.Sum(damage => damage.Amount) - shields;
			if (damageDealt > 0)
				remainingHealth -= damageDealt;
		}

		public void Repair(int amount)
		{
			var newHealth = RemainingHealth + amount;
			remainingHealth = (newHealth < TotalHealth) ? newHealth : TotalHealth;
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
