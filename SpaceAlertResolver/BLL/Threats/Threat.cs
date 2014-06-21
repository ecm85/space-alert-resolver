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

		public void PlaceOnTrack(Track track)
		{
			PlaceOnTrack(track, track.GetStartingPosition());
		}

		protected virtual void PlaceOnTrack(Track track, int trackPosition)
		{
			Track = track;
			Position = trackPosition;
			HasBeenPlaced = true;
			ThreatController.ThreatsMove += PerformMove;
		}

		public virtual bool IsDamageable { get { return HasBeenPlaced && Position != null; } }

		protected bool HasBeenPlaced { get; private set; }
		public virtual int Points
		{
			get { return !HasBeenPlaced ? 0 : IsDefeated ? GetPointsForDefeating() : IsSurvived ? GetPointsForSurviving() : 0; }
		}

		protected Track Track { get; set; }

		public virtual int GetPointsForDefeating()
		{
			return ThreatPoints.GetPointsForDefeating(type, difficulty);
		}

		public virtual int GetPointsForSurviving()
		{
			return ThreatPoints.GetPointsForSurviving(type, difficulty);
		}

		public virtual bool IsDefeated { get; protected set; }
		public virtual bool IsSurvived { get; private set; }

		public int TimeAppears { get; set; }
		protected int TotalHealth { get; private set; }
		protected int RemainingHealth { get; set; }
		public int Speed { get; set; }
		public int? Position { get; protected set; }
		protected ThreatController ThreatController { get; set; }

		private readonly ThreatType type;
		private readonly ThreatDifficulty difficulty;

		protected ISittingDuck SittingDuck { get; set; }

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
			ThreatController.ThreatsMove -= PerformMove;
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
			this.difficulty = difficulty;
			this.type = type;
			TotalHealth = RemainingHealth = health;
			Speed = speed;
		}

		protected void PerformMove(int currentTurn)
		{
			PerformMove(currentTurn, Speed);
		}

		protected void PerformMove(int currentTurn, int amount)
		{
			BeforeMove();
			var newPosition = Position - amount;
			//TODO: Red threats: Need to fix this to allow jumpers (need to get all the breakpoints before performing any
			//And perhaps need to not perform certain breakpoints if threat dies after one of them
			while (Position != null && Position > newPosition)
			{
				var crossedBreakpoint = Track.MoveSingle(Position.Value);
				Position--;
				if (crossedBreakpoint != null)
					switch (crossedBreakpoint.Value)
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
