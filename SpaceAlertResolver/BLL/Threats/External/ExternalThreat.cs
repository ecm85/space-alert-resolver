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

		protected ExternalThreat(int pointsForSurviving, int pointsForDefeating, int shields, int health, int speed, int timeAppears, Zone currentZone) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears)
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
	}
}
