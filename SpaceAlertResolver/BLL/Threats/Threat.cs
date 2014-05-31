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
		public int TotalHealth { get; private set; }
		public int RemainingHealth { get; protected set; }
		public int Speed { get; protected set; }

		private readonly ThreatType type;
		private readonly ThreatDifficulty difficulty;

		protected readonly SittingDuck sittingDuck;

		public abstract void PeformXAction();
		public abstract void PerformYAction();
		public abstract void PerformZAction();

		public virtual void JumpingToHyperspace()
		{
			
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
	}
}
