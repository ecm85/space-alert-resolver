using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Tracks;

namespace BLL.Threats
{
	public abstract class Threat
	{
		public event EventHandler Moving = (sender, args) => { };
		public event EventHandler Moved = (sender, args) => { };
		public event EventHandler TurnEnded = (sender, args) => { };

		private IList<ThreatStatus> ThreatStatuses { get; } = new List<ThreatStatus>();

		public bool GetThreatStatus(ThreatStatus threatStatus)
		{
			return ThreatStatuses.Contains(threatStatus);
		}

		public void SetThreatStatus(ThreatStatus threatStatus, bool value)
		{
			if (value)
				ThreatStatuses.Add(threatStatus);
			else
				ThreatStatuses.Remove(threatStatus);
		}
		
		public int BuffCount => ThreatStatuses.Count(threatStatus => threatStatus.IsBuff());
		public int DebuffCount => ThreatStatuses.Count(threatStatus => threatStatus.IsDebuff());

		public void PlaceOnBoard(Track track)
		{
			PlaceOnBoard(track, track.StartingPosition);
		}

		public virtual void PlaceOnBoard(Track track, int trackPosition)
		{
			SetThreatStatus(ThreatStatus.NotAppeared, false);
			SetThreatStatus(ThreatStatus.OnTrack, true);
			Track = track;
			Position = trackPosition;
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

		public bool IsDefeated => GetThreatStatus(ThreatStatus.Defeated);
		public bool IsSurvived => GetThreatStatus(ThreatStatus.Survived);
		private bool HasAppeared => !GetThreatStatus(ThreatStatus.NotAppeared);

		public virtual bool IsMoveable => IsOnTrack;
		public bool IsOnTrack => GetThreatStatus(ThreatStatus.OnTrack);

		public int Points => !HasAppeared ? 0 : IsDefeated ? PointsForDefeating: IsSurvived ? PointsForSurviving: 0;

		public virtual bool NeedsBonusExternalThreat => false;
		public virtual bool NeedsBonusInternalThreat => false;

		protected Track Track { get; set; }

		public virtual int PointsForDefeating => ThreatPoints.GetPointsForDefeating(ThreatType, Difficulty);

		protected virtual int PointsForSurviving => ThreatPoints.GetPointsForSurviving(ThreatType, Difficulty);

		public int TimeAppears { get; set; }
		protected int TotalHealth { get; }
		public int RemainingHealth { get; protected set; }
		public int Speed { get; set; }
		public int Position { get; private set; }
		protected ThreatController ThreatController { get; private set; }

		public ThreatType ThreatType { get; }
		public ThreatDifficulty Difficulty { get; }

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
			if(IsSurvivedWhenReachingEndOfTrack)
				SetThreatStatus(ThreatStatus.Survived, true);
			OnThreatTerminated();
		}

		protected virtual void OnHealthReducedToZero()
		{
			if(IsDefeatedWhenHealthReachesZero)
				SetThreatStatus(ThreatStatus.Defeated, true);
			OnThreatTerminated();
		}

		protected virtual bool IsDefeatedWhenHealthReachesZero => true;
		protected virtual bool IsSurvivedWhenReachingEndOfTrack => true;
		
		protected virtual void OnThreatTerminated()
		{
			SetThreatStatus(ThreatStatus.OnTrack, false);
			ThreatController.TurnEnding -= OnTurnEnded;
		}

		protected bool IsDamaged => RemainingHealth < TotalHealth;

		public abstract string Id { get; }
		public abstract string DisplayName { get; }
		public abstract string FileName { get; }

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
			var crossedBreakpoints = Track.GetCrossedBreakpoints(oldPosition, newPosition);
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
