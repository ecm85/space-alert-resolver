using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class MinorWhiteInternalThreat : MinorInternalThreat
	{
		protected MinorWhiteInternalThreat(int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck) :
			base(ThreatDifficulty.White, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		protected MinorWhiteInternalThreat(int health, int speed, int timeAppears, IList<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck) :
			base(ThreatDifficulty.White, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}
	}
}
