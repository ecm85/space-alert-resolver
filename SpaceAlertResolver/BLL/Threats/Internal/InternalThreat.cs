using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		public List<StationLocation> CurrentStations { get; private set; }

		private readonly int? totalInaccessibility;
		private int? remainingInaccessibility;

		protected StationLocation CurrentStation
		{
			get { return CurrentStations.Single(); }
			set { CurrentStations = new List<StationLocation>{value}; }
		}

		protected InternalTrack Track { get; private set; }
		public override bool IsSurvived { get { return !IsDestroyed && (!Track.ThreatPositions.ContainsKey(this) || Track.ThreatPositions[this] <= 0); } }

		public void SetTrack(InternalTrack track)
		{
			Track = track;
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

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck, int? inaccessibility = null) :
			this(type, difficulty, health, speed, timeAppears, new List<StationLocation> {currentStation}, actionType, sittingDuck, inaccessibility)
		{
		}

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, List<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck, int? inaccessibility = null) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStations = currentStations;
			sittingDuck.AddInternalThreatToStations(CurrentStations, this);
			ActionType = actionType;
			totalInaccessibility = remainingInaccessibility = inaccessibility;
		}

		//TODO: Respect isHeroic here instead of in the ship
		public virtual void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
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
			CheckForDestroyed();
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
			sittingDuck.RemoveInternalThreatFromStations(new [] {stationToRemoveFrom}, this);
			CurrentStations.Remove(stationToRemoveFrom);
		}

		protected void AddToStation(StationLocation stationToMoveTo)
		{
			sittingDuck.AddInternalThreatToStations(new [] {stationToMoveTo}, this);
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
			var result = sittingDuck.TakeAttack(new ThreatDamage(amount, ThreatDamageType.Internal, zones));
			if (result.ShipDestroyed)
				throw new LoseException(this);
		}

		public virtual void PerformEndOfPlayerActions()
		{
		}

		public IrreparableMalfunction GetIrreparableMalfunction()
		{
			if (ActionType == PlayerAction.BattleBots)
				return null;
			return new IrreparableMalfunction
			{
				ActionType = ActionType
			};
		}

		public override void PerformEndOfTurn()
		{
			base.PerformEndOfTurn();
			remainingInaccessibility = totalInaccessibility;
		}
	}
}
