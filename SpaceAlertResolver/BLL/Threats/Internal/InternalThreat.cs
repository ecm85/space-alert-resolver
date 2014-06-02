using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		public IList<Station> CurrentStations { get; private set; }

		protected Station CurrentStation
		{
			get { return CurrentStations.Single(); }
			set { CurrentStations = new[] {value}; }
		}

		public PlayerAction ActionType { get; private set; }

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStation = currentStation;
			currentStation.Threats.Add(this);
			ActionType = actionType;
		}

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, IList<Station> currentStations, PlayerAction actionType, SittingDuck sittingDuck) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStations = currentStations;
			foreach (var currentStation in CurrentStations)
				currentStation.Threats.Add(this);
			ActionType = actionType;
		}

		protected Zone CurrentZone
		{
			get { return sittingDuck.ZonesByLocation[CurrentStation.ZoneLocation]; }
		}

		public virtual void TakeDamage(int damage, Player performingPlayer, bool isHeroic)
		{
			RemainingHealth -= damage;
			CheckForDestroyed();
		}

		private void MoveToNewStation(Station newStation)
		{
			if (CurrentStations.Count != 1)
				throw new InvalidOperationException("Cannot move a threat that exists in more than 1 zone.");
			CurrentStation.Threats.Remove(this);
			CurrentStation = newStation;
			CurrentStation.Threats.Add(this);
		}

		protected void MoveRed()
		{
			MoveToNewStation(CurrentStation.OppositeDeckStation);
		}

		protected void MoveBlue()
		{
			MoveToNewStation(CurrentStation.BluewardStation);
		}

		protected void ChangeDecks()
		{
			MoveToNewStation(CurrentStation.OppositeDeckStation);
		}

		protected void Damage(int amount)
		{
			DamageZone(amount, CurrentZone);
		}

		protected void DamageOtherTwoZones(int amount)
		{
			DamageZones(amount, sittingDuck.Zones.Except(new [] {CurrentZone}));
		}

		private void DamageZones(int amount, IEnumerable<Zone> zones)
		{
			foreach (var zone in zones)
				DamageZone(amount, zone);
		}

		private void DamageZone(int amount, Zone zone)
		{
			var result = zone.TakeDamage(amount);
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
	}
}
