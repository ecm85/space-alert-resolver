using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class MinorInternalThreat : InternalThreat
	{
		protected MinorInternalThreat(int pointsForSurviving, int pointsForDefeating, int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
			threatType = ThreatType.MinorExternal;
		}

		protected MinorInternalThreat(int pointsForSurviving, int pointsForDefeating, int health, int speed, int timeAppears, IList<Station> currentStations, PlayerAction actionType, SittingDuck sittingDuck) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
			threatType = ThreatType.MinorExternal;
		}
	}
}
