using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats
{
	public abstract class Threat
	{
		public event Action BeforeMove = () => { };
		public event Action AfterMove = () => { };

		public void PlaceOnBoard(Track track)
		{
			PlaceOnBoard(track, track.GetStartingPosition());
		}

		public virtual void PlaceOnBoard(Track track, int? trackPosition)
		{
			Track = track;
			Position = trackPosition;
			HasBeenPlaced = true;
		}

		public void Initialize(ISittingDuck sittingDuck, ThreatController threatController)
		{
			SittingDuck = sittingDuck;
			ThreatController = threatController;
		}

		public virtual bool IsDamageable { get { return HasBeenPlaced && Position != null; } }
		public virtual bool IsMoveable { get { return HasBeenPlaced && Position != null; } }
		public bool IsOnTrack { get { return HasBeenPlaced && Position != null; } }

		private bool HasBeenPlaced { get; set; }

		public virtual int Points
		{
			get { return !HasBeenPlaced ? 0 : IsDefeated ? GetPointsForDefeating() : IsSurvived ? GetPointsForSurviving() : 0; }
		}

		public virtual bool NeedsBonusExternalThreat { get { return false; } }
		public virtual bool NeedsBonusInternalThreat { get { return false; } }

		protected Track Track { get; set; }

		public virtual int GetPointsForDefeating()
		{
			return ThreatPoints.GetPointsForDefeating(Type, Difficulty);
		}

		protected virtual int GetPointsForSurviving()
		{
			return ThreatPoints.GetPointsForSurviving(Type, Difficulty);
		}

		public virtual bool IsDefeated { get; protected set; }
		public virtual bool IsSurvived { get; private set; }

		public int TimeAppears { get; set; }
		protected int TotalHealth { get; private set; }
		protected int RemainingHealth { get; set; }
		public int Speed { get; set; }
		public int? Position { get; private set; }
		protected ThreatController ThreatController { get; private set; }

		public ThreatType Type { get; private set; }
		protected readonly ThreatDifficulty Difficulty;

		protected ISittingDuck SittingDuck { get; private set; }

		protected abstract void PerformXAction(int currentTurn);
		protected abstract void PerformYAction(int currentTurn);
		protected abstract void PerformZAction(int currentTurn);

		protected void CheckDefeated()
		{
			if (RemainingHealth <= 0)
				OnHealthReducedToZero();
		}

		protected virtual void OnReachingEndOfTrack()
		{
			IsSurvived = true;
			OnThreatTerminated();
		}

		protected virtual void OnHealthReducedToZero()
		{
			IsDefeated = true;
			OnThreatTerminated();
		}

		protected virtual void OnThreatTerminated()
		{
			Position = null;
		}

		protected bool IsDamaged
		{
			get { return RemainingHealth < TotalHealth; }
		}

		public void Repair(int amount)
		{
			var newHealth = RemainingHealth + amount;
			RemainingHealth = (newHealth < TotalHealth) ? newHealth : TotalHealth;
		}

		protected Threat(ThreatType type, ThreatDifficulty difficulty, int health, int speed)
		{
			Difficulty = difficulty;
			Type = type;
			TotalHealth = RemainingHealth = health;
			Speed = speed;
		}

		public void Move(int currentTurn)
		{
			var amount = Speed;
			if (ThreatController.CurrentExternalThreatBuffs().Contains(ExternalThreatEffect.ReducedMovement))
				amount -= 1;
			Move(currentTurn, amount);
		}

		public void Move(int currentTurn, int amount)
		{
			BeforeMove();
			var oldPosition = Position;
			Position -= amount;
			var newPosition = Position;
			var crossedBreakpoints = Track.GetCrossedBreakpoints(oldPosition.GetValueOrDefault(), newPosition.GetValueOrDefault());
			foreach (var breakpoint in crossedBreakpoints)
			{
				switch (breakpoint)
					{
						case TrackBreakpointType.X:
							PerformXAction(currentTurn);
							break;
						case TrackBreakpointType.Y:
							PerformYAction(currentTurn);
							break;
						case TrackBreakpointType.Z:
							PerformZAction(currentTurn);
							OnReachingEndOfTrack();
							break;
					}
			}
			AfterMove();
		}
	}
}
