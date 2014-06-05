using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.SeriousExternal.White
{
	public abstract class SeriousWhiteExternalThreat : SeriousExternalThreat
	{
		protected SeriousWhiteExternalThreat(int shields, int health, int speed, int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck) :
			base(ThreatDifficulty.White, shields, health, speed, timeAppears, currentZone, sittingDuck)
		{
		}
	}
}
