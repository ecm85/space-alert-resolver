using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class Threat
	{
		public virtual int PointsForDefeating { get { return ThreatPoints.GetPointsForDefeating(type, difficulty); } }
		public virtual int PointsForSurviving { get { return ThreatPoints.GetPointsForSurviving(type, difficulty); } }

		public int TimeAppears { get; private set; }
		private int TotalHealth { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; protected set; }

		private readonly ThreatType type;
		private readonly ThreatDifficulty difficulty;

		protected readonly SittingDuck sittingDuck;

		private bool destroyed = false;

		public abstract void PeformXAction();
		public abstract void PerformYAction();
		public abstract void PerformZAction();

		//TODO: Call this appropriately
		public virtual void OnJumpingToHyperspace()
		{
			
		}

		public virtual void CheckForDestroyed()
		{
			if (!destroyed && RemainingHealth <= 0)
				OnDestroyed();
		}

		public virtual void OnDestroyed()
		{
			destroyed = true;
		}

		protected bool IsDamaged
		{
			get { return RemainingHealth < TotalHealth; }
		}

		protected Threat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, SittingDuck sittingDuck)
		{
			this.difficulty = difficulty;
			this.type = type;
			TotalHealth = RemainingHealth = health;
			Speed = speed;
			TimeAppears = timeAppears;
			this.sittingDuck = sittingDuck;
		}

		public void Repair(int amount)
		{
			var newHealth = RemainingHealth + amount;
			RemainingHealth = (newHealth < TotalHealth) ? newHealth : TotalHealth;
		}
	}
}
