using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public abstract class MinorExternalThreat : ExternalThreat
	{
		protected MinorExternalThreat(ThreatDifficulty difficulty, int shields, int health, int speed, int timeAppears, ZoneLocation currentZone, SittingDuck sittingDuck) : 
			base(ThreatType.MinorExternal, difficulty, shields, health, speed, timeAppears, currentZone, sittingDuck)
		{
		}
	}
}
