using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public abstract class MinorYellowInternalThreat : MinorInternalThreat
	{
		protected MinorYellowInternalThreat(int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck)
			: base(ThreatDifficulty.Yellow, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		protected MinorYellowInternalThreat(int health, int speed, int timeAppears, IList<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck)
			: base(ThreatDifficulty.Yellow, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}
	}
}
