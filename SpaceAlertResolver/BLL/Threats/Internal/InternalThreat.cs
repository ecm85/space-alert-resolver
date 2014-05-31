using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		public IList<Station> CurrentStations { get; protected set; } 
		public Station CurrentStation { get { return CurrentStations.Single(); } set { CurrentStations = new[] {value}; } }
		public PlayerAction ActionType { get; private set; }

		protected InternalThreat(int pointsForSurviving, int pointsForDefeating, int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears, sittingDuck)
		{
			CurrentStation = currentStation;
			ActionType = actionType;
		}

		//TODO: Revisit all the ctors arguments
		protected InternalThreat(int pointsForSurviving, int pointsForDefeating, int health, int speed, int timeAppears, IList<Station> currentStations, PlayerAction actionType, SittingDuck sittingDuck) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears, sittingDuck)
		{
			CurrentStations = currentStations;
			foreach (var currentStation in CurrentStations)
				currentStation.Threats.Add(this);
			ActionType = actionType;
		}

		public virtual InternalPlayerDamageResult TakeDamage(int damage)
		{
			remainingHealth -= damage;
			return new InternalPlayerDamageResult();
		}

		protected void MoveToNewStation(Station newStation)
		{
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
