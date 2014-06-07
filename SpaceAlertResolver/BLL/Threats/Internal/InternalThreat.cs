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

		protected readonly int? totalInaccessibility;
		protected int? remainingInaccessibility;

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
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStation = currentStation;
			sittingDuck.StationByLocation[currentStation].Threats.Add(this);
			ActionType = actionType;
			totalInaccessibility = remainingInaccessibility = inaccessibility;
		}

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, List<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck, int? inaccessibility = null) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStations = currentStations;
			foreach (var currentStation in CurrentStations)
				sittingDuck.StationByLocation[currentStation].Threats.Add(this);
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

		private void MoveToNewStation(StationLocation newStation)
		{
			if (CurrentStations.Count != 1)
				throw new InvalidOperationException("Cannot move a threat that exists in more than 1 zone.");
			RemoveFromStation(CurrentStation);
			AddToStation(newStation);
		}

		protected void RemoveFromStation(StationLocation stationToRemoveFrom)
		{
			sittingDuck.StationByLocation[stationToRemoveFrom].Threats.Remove(this);
			CurrentStations.Remove(stationToRemoveFrom);
		}

		protected void AddToStation(StationLocation stationToMoveTo)
		{
			sittingDuck.StationByLocation[stationToMoveTo].Threats.Add(this);
			CurrentStations.Add(stationToMoveTo);
		}

		protected void MoveRed()
		{
			MoveToNewStation(sittingDuck.StationByLocation[CurrentStation].RedwardStation.StationLocation);
		}

		protected void MoveBlue()
		{
			MoveToNewStation(sittingDuck.StationByLocation[CurrentStation].BluewardStation.StationLocation);
		}

		protected void ChangeDecks()
		{
			MoveToNewStation(sittingDuck.StationByLocation[CurrentStation].OppositeDeckStation.StationLocation);
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
