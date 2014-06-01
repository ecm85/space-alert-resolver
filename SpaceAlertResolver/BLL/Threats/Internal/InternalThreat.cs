using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		protected IList<IStation> CurrentStations { get; private set; }
		protected IStation CurrentStation { get { return CurrentStations.Single(); } private set { CurrentStations = new[] {value}; } }
		public PlayerAction ActionType { get; private set; }

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, IStation currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStation = currentStation;
			ActionType = actionType;
		}

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, IList<IStation> currentStations, PlayerAction actionType, SittingDuck sittingDuck) :
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

		public virtual InternalPlayerDamageResult TakeDamage(int damage, Player performingPlayer)
		{
			RemainingHealth -= damage;
			CheckForDestroyed();
			return new InternalPlayerDamageResult();
		}

		private void MoveToNewStation(IStation newStation)
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
			sittingDuck.TakeDamage(amount, CurrentStation.ZoneLocation);
		}

		protected void DamageOtherTwoZones(int amount)
		{
			DamageZones(amount, sittingDuck.Zones.Except(new [] {CurrentZone}));
		}

		private void DamageZones(int amount, IEnumerable<Zone> zones)
		{
			foreach (var zone in zones)
				zone.TakeDamage(amount);
		}

		public virtual void PerformEndOfPlayerActions()
		{
		}

	}
}
