using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class InternalThreat : Threat
	{
		public Station CurrentStation { get; protected set; }
		public PlayerAction ActionType { get; private set; }

		protected InternalThreat(int pointsForSurviving, int pointsForDefeating, int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears)
		{
			CurrentStation = currentStation;
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
