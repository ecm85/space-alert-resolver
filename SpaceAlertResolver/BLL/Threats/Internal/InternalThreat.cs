using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		//TODO: Clean up single vs multiple stations

		protected List<StationLocation> CurrentStations { get; private set; }

		private readonly int? totalInaccessibility;
		private int? remainingInaccessibility;

		protected StationLocation CurrentStation
		{
			get { return CurrentStations.Single(); }
			set { CurrentStations = new List<StationLocation>{value}; }
		}

		protected InternalTrack Track { get; private set; }

		public void PlaceOnTrack(InternalTrack track)
		{
			PlaceOnTrack(track, track.GetStartingPosition());
		}

		protected void PlaceOnTrack(InternalTrack track, int? trackPosition)
		{
			Track = track;
			Position = trackPosition;
			HasBeenPlaced = true;
		}

		protected ZoneLocation CurrentZone
		{
			get { return CurrentStation.ZoneLocation(); }
		}

		protected IList<ZoneLocation> CurrentZones
		{
			get { return CurrentStations.Select(station => station.ZoneLocation()).ToList(); }
		}


		public PlayerAction ActionType { get; private set; }

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerAction actionType, int? inaccessibility = null) :
			this(type, difficulty, health, speed, new List<StationLocation> {currentStation}, actionType, inaccessibility)
		{
		}

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, List<StationLocation> currentStations, PlayerAction actionType, int? inaccessibility = null) :
			base(type, difficulty, health, speed)
		{
			CurrentStations = currentStations;
			ActionType = actionType;
			totalInaccessibility = remainingInaccessibility = inaccessibility;
		}

		public virtual void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears)
		{
			SittingDuck = sittingDuck;
			sittingDuck.AddInternalThreatToStations(CurrentStations, this);
			ThreatController = threatController;
			TimeAppears = timeAppears;
		}

		public void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			if (!IsOnTrack())
				return;
			TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
		}

		//TODO: Respect isHeroic here instead of in the ship
		protected virtual void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
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
			RemoveFromStation(CurrentStation);
			AddToStation(newStation.Value);
		}

		protected void RemoveFromStation(StationLocation stationToRemoveFrom)
		{
			SittingDuck.RemoveInternalThreatFromStations(new [] {stationToRemoveFrom}, this);
			CurrentStations.Remove(stationToRemoveFrom);
		}

		protected void AddToStation(StationLocation stationToMoveTo)
		{
			SittingDuck.AddInternalThreatToStations(new [] {stationToMoveTo}, this);
			CurrentStations.Add(stationToMoveTo);
		}

		protected void MoveRed()
		{
			MoveToNewStation(CurrentStation.RedwardStationLocation());
		}

		protected void MoveBlue()
		{
			MoveToNewStation(CurrentStation.BluewardStationLocation());
		}

		protected void ChangeDecks()
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
			var result = SittingDuck.TakeAttack(new ThreatDamage(amount, ThreatDamageType.Internal, zones));
			if (result.ShipDestroyed)
				throw new LoseException(this);
		}

		public void PerformEndOfPlayerActions()
		{
			if (!IsOnTrack())
				return;
			PerformEndOfPlayerActionsOnTrack();
		}

		protected virtual void PerformEndOfPlayerActionsOnTrack()
		{
		}

		public override void OnReachingEndOfTrack()
		{
			base.OnReachingEndOfTrack();
			SittingDuck.RemoveInternalThreatFromStations(CurrentStations, this);
			CurrentStations.Clear();
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			SittingDuck.RemoveInternalThreatFromStations(CurrentStations, this);
			CurrentStations.Clear();
		}

		protected override void OnHealthReducedToZero(bool clearPosition)
		{
			base.OnHealthReducedToZero(clearPosition);
			SittingDuck.RemoveInternalThreatFromStations(CurrentStations, this);
			CurrentStations.Clear();
		}

		//TODO: Add irreparable malfunctions to ship here
		public IrreparableMalfunction GetIrreparableMalfunction()
		{
			if (ActionType == PlayerAction.BattleBots)
				return null;
			return new IrreparableMalfunction
			{
				ActionType = ActionType
			};
		}

		protected override void PerformEndOfTurnOnTrack()
		{
			base.PerformEndOfTurnOnTrack();
			remainingInaccessibility = totalInaccessibility;
		}

		public override void Move()
		{
			Move(Speed);
		}

		private void Move(int amount)
		{
			if (!IsOnTrack())
				return;
			BeforeMove();
			Track.MoveThreat(this, amount);
			AfterMove();
		}
	}
}
