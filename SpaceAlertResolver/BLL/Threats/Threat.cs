using System;
using System.Linq;
using BLL.Tracks;

namespace BLL.Threats
{
	public abstract class Threat
	{
		public event EventHandler Moving = (sender, args) => { };
		public event EventHandler Moved = (sender, args) => { };
		public event EventHandler TurnEnded = (sender, args) => { };

		public void PlaceOnBoard(Track track)
		{
			PlaceOnBoard(track, track.StartingPosition);
		}

		public virtual void PlaceOnBoard(Track track, int? trackPosition)
		{
			Track = track;
			Position = trackPosition;
			HasBeenPlaced = true;
			TurnEnded += OnTurnEnded;
			ThreatController.TurnEnding += (sender, args) => TurnEnded(sender, args);
		}

		protected virtual void OnTurnEnded(object sender, EventArgs args)
		{
		}

		public void Initialize(ISittingDuck sittingDuck, ThreatController threatController)
		{
			SittingDuck = sittingDuck;
			ThreatController = threatController;
		}

		public virtual bool IsDamageable => HasBeenPlaced && Position != null;
		public virtual bool IsMoveable => HasBeenPlaced && Position != null;
		public bool IsOnTrack => HasBeenPlaced && Position != null;

		private bool HasBeenPlaced { get; set; }

		public virtual int Points => !HasBeenPlaced ? 0 : IsDefeated ? PointsForDefeating: IsSurvived ? PointsForSurviving: 0;

		public virtual bool NeedsBonusExternalThreat => false;
		public virtual bool NeedsBonusInternalThreat => false;

		protected Track Track { get; set; }

		public virtual int PointsForDefeating => ThreatPoints.GetPointsForDefeating(ThreatType, Difficulty);

		protected virtual int PointsForSurviving => ThreatPoints.GetPointsForSurviving(ThreatType, Difficulty);

		public virtual bool IsDefeated { get; protected set; }
		public virtual bool IsSurvived { get; private set; }

		public int TimeAppears { get; set; }
		protected int TotalHealth { get; }
		protected int RemainingHealth { get; set; }
		public int Speed { get; set; }
		public int? Position { get; private set; }
		protected ThreatController ThreatController { get; private set; }

		public ThreatType ThreatType { get; }
		protected ThreatDifficulty Difficulty { get; }

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
			ThreatController.TurnEnding -= OnTurnEnded;
		}

		protected bool IsDamaged => RemainingHealth < TotalHealth;

		public void Repair(int amount)
		{
			var newHealth = RemainingHealth + amount;
			RemainingHealth = (newHealth < TotalHealth) ? newHealth : TotalHealth;
		}

		protected Threat(ThreatType threatType, ThreatDifficulty difficulty, int health, int speed)
		{
			Difficulty = difficulty;
			ThreatType = threatType;
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
			Moving(null, null);
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
			Moved(null, null);
		}
	}
}
