using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public abstract class MinorYellowExternalThreat : MinorExternalThreat
	{
		protected MinorYellowExternalThreat(int shields, int health, int speed, int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(ThreatDifficulty.Yellow, shields, health, speed, timeAppears, currentZone, sittingDuck)
		{
		}
	}
}
