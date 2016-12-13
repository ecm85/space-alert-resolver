using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		protected IList<StationLocation> CurrentStations { get; private set; }

		private int? totalInaccessibility;
		private int? remainingInaccessibility;

		protected void SetTotalInaccessibility(int totalInaccessibility)
		{
			this.totalInaccessibility = totalInaccessibility;
		}

		internal StationLocation CurrentStation
		{
			get { return CurrentStations.Single(); }
			set { CurrentStations = new List<StationLocation>{value}; }
		}

		protected ZoneLocation CurrentZone => CurrentStation.ZoneLocation();

		protected IList<ZoneLocation> CurrentZones
		{
			get { return CurrentStations.Select(station => station.ZoneLocation()).ToList(); }
		}

		protected PlayerActionType? ActionType { get; }

		protected InternalThreat(ThreatType threatType, ThreatDifficulty difficulty, int health, int speed, IList<StationLocation> currentStations) :
			base(threatType, difficulty, health, speed)
		{
			CurrentStations = currentStations;

		}

		protected InternalThreat(ThreatType threatType, ThreatDifficulty difficulty, int health, int speed, IList<StationLocation> currentStations, PlayerActionType? actionType) :
			this(threatType, difficulty, health, speed, currentStations)
		{
			ActionType = actionType;
		}

		protected InternalThreat(ThreatType threatType, ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerActionType? actionType) :
			this(threatType, difficulty, health, speed, new List<StationLocation> {currentStation}, actionType)
		{
		}

		protected InternalThreat(ThreatType threatType, ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerActionType? actionType, int? inaccessibility) :
			this(threatType, difficulty, health, speed, new List<StationLocation> {currentStation}, actionType)
		{
			totalInaccessibility = remainingInaccessibility = inaccessibility;
		}

		public virtual void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			var damageRemaining = damage;
			if (remainingInaccessibility.HasValue)
			{
				var previousInaccessibility = remainingInaccessibility.Value;
				remainingInaccessibility -= damageRemaining;
				remainingInaccessibility = remainingInaccessibility < 0 ? 0 : remainingInaccessibility;
				damageRemaining -= previousInaccessibility;
			}
			if (damageRemaining > 0)
				RemainingHealth -= damage;
			CheckDefeated();
		}

		private void MoveToNewStation(StationLocation? newStation)
		{
			if (CurrentStations.Count != 1)
				throw new InvalidOperationException("Cannot move a threat that exists in more than 1 zone.");
			if (!newStation.HasValue)
				throw new InvalidOperationException("Moved wrongly.");
			CurrentStations.Remove(CurrentStation);
			CurrentStations.Add(newStation.Value);
		}

		internal void MoveRed()
		{
			if (CanMoveRed())
				MoveToNewStation(CurrentStation.RedwardStationLocation());
		}

		private bool CanMoveRed()
		{
			switch (CurrentZone)
			{
				case ZoneLocation.Red:
					return false;
				case ZoneLocation.White:
					return !SittingDuck.RedAirlockIsBreached;
				case ZoneLocation.Blue:
					return !SittingDuck.BlueAirlockIsBreached;
				default:
					throw new InvalidOperationException("Invalid zone location encountered");
			}
		}

		internal void MoveBlue()
		{
			if (CanMoveBlue())
				MoveToNewStation(CurrentStation.BluewardStationLocation());
		}

		private bool CanMoveBlue()
		{
			switch (CurrentZone)
			{
				case ZoneLocation.Blue:
					return false;
				case ZoneLocation.White:
					return !SittingDuck.BlueAirlockIsBreached;
				case ZoneLocation.Red:
					return !SittingDuck.RedAirlockIsBreached;
				default:
					throw new InvalidOperationException("Invalid zone location encountered");
			}
		}

		internal void ChangeDecks()
		{
			MoveToNewStation(CurrentStation.OppositeStationLocation());
		}

		protected void Damage(int amount)
		{
			Damage(amount, new [] {CurrentZone});
		}

		protected void DamageOtherTwoZones(int amount)
		{
			Damage(amount, EnumFactory.All<ZoneLocation>().Except(new[] { CurrentZone }).ToList());
		}

		protected void DamageAllZones(int amount)
		{
			Damage(amount, EnumFactory.All<ZoneLocation>());
		}

		protected void Damage(int amount, IList<ZoneLocation> zones)
		{
			var result = SittingDuck.TakeAttack(new ThreatDamage(amount, ThreatDamageType.IgnoresShields, zones));
			if (result.ShipDestroyed)
				throw new LoseException(this);
		}

		protected override void OnReachingEndOfTrack()
		{
			base.OnReachingEndOfTrack();
			AddIrreparableMalfunction();
		}

		private void AddIrreparableMalfunction()
		{
			if (ActionType!= null && ActionType != PlayerActionType.BattleBots)
				SittingDuck.AddIrreparableMalfunctionToStations(
					CurrentStations,
					new IrreparableMalfunction {ActionType = ActionType.Value});
		}

		protected override void OnTurnEnded(object sender, EventArgs args)
		{
			base.OnTurnEnded(sender, args);
			remainingInaccessibility = totalInaccessibility;
		}

		public virtual bool CanBeTargetedBy(StationLocation stationLocation, PlayerActionType playerActionType, Player performingPlayer)
		{
			return IsDamageable && ActionType == playerActionType && CurrentStations.Contains(stationLocation);
		}

		public bool NextDamageWillDestroyThreat()
		{
			return remainingInaccessibility == 0 && RemainingHealth == 1;
		}
	}
}
