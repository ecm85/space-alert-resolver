using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious
{
	public abstract class SeriousExternalThreat : ExternalThreat
	{
		protected SeriousExternalThreat(ThreatDifficulty difficulty, int shields, int health, int speed, int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(ThreatType.SeriousExternal, difficulty, shields, health, speed, timeAppears, currentZone, sittingDuck)
		{
		}
	}
}
