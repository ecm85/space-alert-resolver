using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class Threat
	{
		public int PointsForDefeating { get { return ThreatPoints.GetPointsForDefeating(type, difficulty); } }
		public int PointsForSurviving { get { return ThreatPoints.GetPointsForSurviving(type, difficulty); } }

		public int TimeAppears { get; private set; }
		protected int TotalHealth { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; protected set; }

		private readonly ThreatType type;
		private readonly ThreatDifficulty difficulty;

		protected readonly ISittingDuck sittingDuck;

		private bool destroyed;

		public abstract void PeformXAction();
		public abstract void PerformYAction();
		public abstract void PerformZAction();

		public virtual void OnJumpingToHyperspace()
		{
		}

		public void CheckForDestroyed()
		{
			if (!destroyed && RemainingHealth <= 0)
				OnDestroyed();
		}

		protected virtual void OnDestroyed()
		{
			destroyed = true;
		}

		protected bool IsDamaged
		{
			get { return RemainingHealth < TotalHealth; }
		}

		protected Threat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, ISittingDuck sittingDuck)
		{
			this.difficulty = difficulty;
			this.type = type;
			TotalHealth = RemainingHealth = health;
			Speed = speed;
			TimeAppears = timeAppears;
			this.sittingDuck = sittingDuck;
		}

		protected void Repair(int amount)
		{
			var newHealth = RemainingHealth + amount;
			RemainingHealth = (newHealth < TotalHealth) ? newHealth : TotalHealth;
		}
	}
}
