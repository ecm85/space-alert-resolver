using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		//TODO: Extact common internal threat actions, like external
		public IList<Station> CurrentStations { get; protected set; } 
		public Station CurrentStation { get { return CurrentStations.Single(); } set { CurrentStations = new[] {value}; } }
		public PlayerAction ActionType { get; private set; }

		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStation = currentStation;
			ActionType = actionType;
		}

		//TODO: Revisit all the ctors arguments
		protected InternalThreat(ThreatType type, ThreatDifficulty difficulty, int health, int speed, int timeAppears, IList<Station> currentStations, PlayerAction actionType, SittingDuck sittingDuck) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			CurrentStations = currentStations;
			foreach (var currentStation in CurrentStations)
				currentStation.Threats.Add(this);
			ActionType = actionType;
		}

		public virtual InternalPlayerDamageResult TakeDamage(int damage)
		{
			RemainingHealth -= damage;
			CheckForDestroyed();
			return new InternalPlayerDamageResult();
		}

		protected void MoveToNewStation(Station newStation)
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
	}
}
