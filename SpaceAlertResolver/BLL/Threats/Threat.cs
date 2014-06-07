using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class Threat
	{
		public virtual int Points
		{
			get { return IsDefeated ? GetPointsForDefeating() : IsSurvived ? GetPointsForSurviving() : 0; }
		}

		protected virtual int GetPointsForDefeating()
		{
			return ThreatPoints.GetPointsForDefeating(type, difficulty);
		}

		protected virtual int GetPointsForSurviving()
		{
			return ThreatPoints.GetPointsForSurviving(type, difficulty);
		}

		public virtual bool IsDefeated {get { return IsDestroyed; }}
		public abstract bool IsSurvived { get; }

		public int TimeAppears { get; private set; }
		protected int TotalHealth { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; protected set; }

		private readonly ThreatType type;
		private readonly ThreatDifficulty difficulty;

		protected readonly ISittingDuck sittingDuck;

		public bool IsDestroyed { get; set; }

		public abstract void PeformXAction();
		public abstract void PerformYAction();
		public abstract void PerformZAction();

		public virtual void OnJumpingToHyperspace()
		{
		}

		public virtual void CheckForDestroyed()
		{
			if (!IsDestroyed && RemainingHealth <= 0)
				OnDestroyed();
		}

		protected virtual void OnDestroyed()
		{
			IsDestroyed = true;
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

		public virtual void PerformEndOfTurn()
		{
		}

		public virtual void BeforeMove()
		{
		}

		public virtual void AfterMove()
		{
		}
	}
}
