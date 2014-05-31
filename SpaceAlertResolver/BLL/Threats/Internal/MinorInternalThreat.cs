using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class MinorInternalThreat : InternalThreat
	{
		protected MinorInternalThreat(int pointsForSurviving, int pointsForDefeating, int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType) :
			base(pointsForSurviving, pointsForDefeating, health, speed, timeAppears, currentStation, actionType)
		{
			threatType = ThreatType.MinorExternal;
		}
	}
}
