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

		protected InternalThreat(int pointsForSurviving, int pointsForDefeating, int shields, int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType) :
			base(pointsForSurviving, pointsForDefeating, shields, health, speed, timeAppears)
		{
			CurrentStation = currentStation;
			ActionType = actionType;
		}

		public virtual void TakeDamage(int damage)
		{
			var damageDealt = damage - shields;
			if (damageDealt > 0)
				remainingHealth -= damageDealt;
		}
	}
}
