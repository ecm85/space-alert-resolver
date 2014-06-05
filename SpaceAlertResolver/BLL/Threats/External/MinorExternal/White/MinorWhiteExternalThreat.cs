using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.MinorExternal.White
{
	public abstract class MinorWhiteExternalThreat : MinorExternalThreat
	{
		protected MinorWhiteExternalThreat(int shields, int health, int speed, int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck) :
			base(ThreatDifficulty.White, shields, health, speed, timeAppears, currentZone, sittingDuck)
		{
		}
	}
}
