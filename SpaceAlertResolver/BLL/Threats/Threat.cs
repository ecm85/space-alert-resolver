using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats
{
	public abstract class Threat
	{
		public event EventHandler Moving = (sender, args) => { };
		public event EventHandler Moved = (sender, args) => { };
		public event EventHandler TurnEnded = (sender, args) => { };

		public abstract ZoneLocation CurrentZone { get; }

		public int? AmountAttackingFor { get; set; }
		public ZoneLocation? ZoneUnderAttack { get; set; }

		public abstract ThreatDamageType StandardDamageType { get; }
		public abstract int? DamageDistanceToSource { get; }

		public event EventHandler<ThreatDamageEventArgs> AttackedSittingDuck = (sender, args) => { };

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

		public void PlaceOnTrack(Track track)
		{
			PlaceOnTrack(track, track.StartingPosition);
		}

		public virtual void PlaceOnTrack(Track track, int trackPosition)
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

		public void Initialize(ISittingDuck sittingDuck, ThreatController threatController, EventMaster eventMaster)
		{
			EventMaster = eventMaster;
			SittingDuck = sittingDuck;
			ThreatController = threatController;
		}

		protected EventMaster EventMaster { get; private set; }

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

		public int TimeAppears { get; protected set; }
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
			if (ThreatStatuses.Contains(ThreatStatus.ReducedMovement))
				amount -= 1;
			Move(currentTurn, amount);
		}

		public void Move(int currentTurn, int amount)
		{
			EventMaster.LogEvent("Moving");
			Moving(null, null);
			var oldPosition = Position;
			for (var i = 0; i < amount; i++)
			{
				Position--;
				EventMaster.LogEvent("Moved a space.");
			}
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
			EventMaster.LogEvent("Done Moving");
		}

		protected int Attack(int amount, ThreatDamageType? threatDamageType = null)
		{
			return AttackSpecificZone(amount, CurrentZone, threatDamageType);
		}

		protected int AttackSpecificZone(int amount, ZoneLocation zone, ThreatDamageType? threatDamageType = null)
		{
			return AttackSpecificZones(amount, new [] {zone}, threatDamageType);
		}

		protected void AttackOtherTwoZones(int amount, ThreatDamageType? threatDamageType = null)
		{
			AttackSpecificZones(amount, EnumFactory.All<ZoneLocation>().Except(new[] { CurrentZone }).ToList(), threatDamageType);
		}

		protected void AttackAllZones(int amount, ThreatDamageType? threatDamageType = null)
		{
			AttackSpecificZones(amount, EnumFactory.All<ZoneLocation>(), threatDamageType);
		}

		protected int AttackSpecificZones(int amount, IList<ZoneLocation> zones, ThreatDamageType? threatDamageType = null)
		{
			var damageShielded = 0;
			foreach (var zoneLocation in zones)
			{
				AmountAttackingFor = amount;
				ZoneUnderAttack = zoneLocation;
				EventMaster.LogEvent("Attacking");
				var bonusAttacks = GetThreatStatus(ThreatStatus.BonusAttack) ? 1 : 0;
				var damage = new ThreatDamage(amount + bonusAttacks, threatDamageType ?? StandardDamageType, zoneLocation, DamageDistanceToSource);
				AttackedSittingDuck(this, new ThreatDamageEventArgs {ThreatDamage = damage});
				damageShielded += damage.DamageShielded;
			}
			AmountAttackingFor = null;
			EventMaster.LogEvent("Done Attacking");
			return damageShielded;
		}
	}
}
