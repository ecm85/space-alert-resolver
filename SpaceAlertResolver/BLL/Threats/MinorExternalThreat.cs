using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class MinorExternalThreat : ExternalThreat
	{
		protected MinorExternalThreat(int pointsForSurviving, int pointsForDefeating, int shields, int health, int speed, int timeAppears, Zone currentZone) : 
			base(pointsForSurviving, pointsForDefeating, shields, health, speed, timeAppears, currentZone)
		{
			threatType = ThreatType.MinorExternal;
		}
	}
}
