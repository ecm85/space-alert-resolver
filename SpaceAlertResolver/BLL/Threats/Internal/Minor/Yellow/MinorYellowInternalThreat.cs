using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class MinorYellowInternalThreat : MinorInternalThreat
	{
		protected MinorYellowInternalThreat(int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, timeAppears, currentStation, actionType, sittingDuck, accessibility)
		{
		}

		protected MinorYellowInternalThreat(int health, int speed, int timeAppears, List<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, timeAppears, currentStations, actionType, sittingDuck, accessibility)
		{
		}
	}
}
