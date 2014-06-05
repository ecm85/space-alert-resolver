using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor
{
	public abstract class MinorInternalThreat : InternalThreat
	{
		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck) :
			base(ThreatType.MinorExternal, difficulty, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, IList<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck) :
			base(ThreatType.MinorExternal, difficulty, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}
	}
}
