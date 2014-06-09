using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class Threat
	{
		protected bool HasBeenPlaced { get; set; }
		public virtual int Points
		{
			get { return !HasBeenPlaced ? 0 : IsDefeated ? GetPointsForDefeating() : IsSurvived ? GetPointsForSurviving() : 0; }
		}

		protected virtual int GetPointsForDefeating()
		{
			return ThreatPoints.GetPointsForDefeating(type, difficulty);
		}

		protected virtual int GetPointsForSurviving()
		{
			return ThreatPoints.GetPointsForSurviving(type, difficulty);
		}

		protected bool isDefeated;
		public virtual bool IsDefeated { get { return isDefeated; } }
		protected bool isSurvived;
		public virtual bool IsSurvived { get { return isSurvived; } }

		public int TimeAppears { get; protected set; }
		protected int TotalHealth { get; set; }
		public int RemainingHealth { get; set; }
		public int Speed { get; protected set; }
		public int? Position { get; set; }
		public ThreatController ThreatController { get; set; }

		private readonly ThreatType type;
		private readonly ThreatDifficulty difficulty;

		protected ISittingDuck SittingDuck { get; set; }

		public abstract void PerformXAction();
		public abstract void PerformYAction();
		public abstract void PerformZAction();

		public virtual void OnJumpingToHyperspace()
		{
		}

		public virtual void CheckDefeated()
		{
			if (IsOnTrack() && RemainingHealth <= 0)
				OnHealthReducedToZero();
		}

		public virtual void OnReachingEndOfTrack()
		{
			isSurvived = true;
			Position = null;
		}

		protected virtual void OnHealthReducedToZero(bool clearPosition)
		{
			isDefeated = true;
			if (clearPosition)
				Position = null;
		}

		protected virtual void OnHealthReducedToZero()
		{
			OnHealthReducedToZero(true);
		}

		protected bool IsDamaged
		{
			get { return RemainingHealth < TotalHealth; }
		}

		protected void Repair(int amount)
		{
			var newHealth = RemainingHealth + amount;
			RemainingHealth = (newHealth < TotalHealth) ? newHealth : TotalHealth;
		}

		public virtual void PerformEndOfTurn()
		{
			if (!IsOnTrack())
				return;
			PerformEndOfTurnOnTrack();
		}

		protected virtual void PerformEndOfTurnOnTrack()
		{
		}

		protected virtual void BeforeMove()
		{
		}

		protected virtual void AfterMove()
		{
		}

		public abstract void Move();
		
		public virtual bool IsOnTrack()
		{
			return HasBeenPlaced && Position > 0;
		}

		protected Threat(ThreatType type, ThreatDifficulty difficulty, int health, int speed)
		{
			this.difficulty = difficulty;
			this.type = type;
			TotalHealth = RemainingHealth = health;
			Speed = speed;
		}
	}
}
