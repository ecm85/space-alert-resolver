using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public abstract class ExternalThreat : Threat
	{
		public ZoneType CurrentZoneType { get; private set; }

		protected ExternalThreat(int pointsForSurviving, int pointsForDefeating, int shields, int health, int speed, int timeAppears, ZoneType currentZoneType) :
			base(pointsForSurviving, pointsForDefeating, shields, health, speed, timeAppears)
		{
			CurrentZoneType = currentZoneType;
		}
	}
}
