using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class Threat
	{
		private readonly int pointsForDefeating;
		public int PointsForDefeating { get { return pointsForDefeating; } }
		private readonly int pointsForSurviving;
		public int PointsForSurviving { get { return pointsForSurviving; } }

		private readonly int totalHealth;
		public int TotalHealth { get { return totalHealth; } }
		protected int remainingHealth;
		public int RemainingHealth { get { return remainingHealth; } }

		protected int speed;
		public int Speed { get { return speed; } }

		public abstract void PeformXAction();
		public abstract void PerformYAction();
		public abstract void PerformZAction();

		public virtual void JumpingToHyperspace()
		{
			
		}

		public int TimeAppears { get; private set; }

		protected ThreatType threatType;
		public ThreatType ThreatType { get { return threatType; } }

		protected bool IsDamaged
		{
			get { return RemainingHealth < TotalHealth; }
		}

		protected readonly SittingDuck sittingDuck;

		protected Threat(int pointsForSurviving, int pointsForDefeating, int health, int speed, int timeAppears, SittingDuck sittingDuck)
		{
			this.pointsForSurviving = pointsForSurviving;
			this.pointsForDefeating = pointsForDefeating;
			totalHealth = remainingHealth = health;
			this.speed = speed;
			TimeAppears = timeAppears;
			this.sittingDuck = sittingDuck;
		}
	}
}
