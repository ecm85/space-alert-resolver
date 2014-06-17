using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		public List<StationLocation> CurrentStations { get; private set; }

		protected int? totalInaccessibility;
		private int? remainingInaccessibility;

		internal StationLocation CurrentStation
		{
			get { return CurrentStations.Single(); }
			set { CurrentStations = new List<StationLocation>{value}; }
		}

		protected override void PlaceOnTrack(Track track, int trackPosition)
		{
			ThreatController.EndOfTurn += PerformEndOfTurn;
			ThreatController.InternalThreatsMove += PerformMove;
			base.PlaceOnTrack(track, trackPosition);
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
			ThreatController = threatController;
			TimeAppears = timeAppears;
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

		protected void MoveToNewStation(StationLocation? newStation)
		{
			if (CurrentStations.Count != 1)
				throw new InvalidOperationException("Cannot move a threat that exists in more than 1 zone.");
			if (!newStation.HasValue)
				throw new InvalidOperationException("Moved wrongly.");
			CurrentStations.Remove(CurrentStation);
			CurrentStations.Add(newStation.Value);
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
			var result = SittingDuck.TakeAttack(new ThreatDamage(amount, ThreatDamageType.IgnoresShields, zones));
			if (result.ShipDestroyed)
				throw new LoseException(this);
		}

		protected override void OnReachingEndOfTrack()
		{
			base.OnReachingEndOfTrack();
			AddIrreparableMalfunction();
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			CurrentStations.Clear();
			ThreatController.EndOfTurn -= PerformEndOfTurn;
			ThreatController.InternalThreatsMove -= PerformMove;
		}

		private void AddIrreparableMalfunction()
		{
			if (ActionType != PlayerAction.BattleBots)
				SittingDuck.AddIrreparableMalfunctionToStations(
					CurrentStations,
					new IrreparableMalfunction {ActionType = ActionType});
		}

		protected virtual void PerformEndOfTurn()
		{
			remainingInaccessibility = totalInaccessibility;
		}
	}
}
