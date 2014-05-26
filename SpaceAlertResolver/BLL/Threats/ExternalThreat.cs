using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class ExternalThreat
	{
		private readonly int pointsForDefeating;
		public int PointsForDefeating { get { return pointsForDefeating; } }
		private readonly int pointsForSurviving;
		public int PointsForSurviving { get { return pointsForSurviving; } }

		private readonly int totalHealth;
		public int TotalHealth { get { return totalHealth; } }
		private int remainingHealth;
		public int RemainingHealth { get { return remainingHealth; } }
		
		protected int shields;

		private readonly int speed;
		public int Speed { get { return speed; } }

		public abstract void PeformXAction(SittingDuck sittingDuck);
		public abstract void PerformYAction(SittingDuck sittingDuck);
		public abstract void PerformZAction(SittingDuck sittingDuck);

		public int TimeAppears { get; private set; }
		public ZoneType CurrentZoneType { get; private set; }

		public virtual void TakeDamage(IList<Damage> damages)
		{
			remainingHealth -= (damages.Sum(damage => damage.Amount) - shields);
		}

		protected ThreatType threatType;
		public ThreatType ThreatType { get { return threatType; } }

		protected ExternalThreat(int pointsForSurviving, int pointsForDefeating, int shields, int health, int speed, int timeAppears, ZoneType currentZoneType)
		{
			this.pointsForSurviving = pointsForSurviving;
			this.pointsForDefeating = pointsForDefeating;
			this.shields = shields;
			totalHealth = remainingHealth = health;
			this.speed = speed;
			CurrentZoneType = currentZoneType;
			TimeAppears = timeAppears;
		}
	}
}
