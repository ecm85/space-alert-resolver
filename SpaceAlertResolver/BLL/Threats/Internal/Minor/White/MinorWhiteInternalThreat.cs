using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class MinorWhiteInternalThreat : MinorInternalThreat
	{
		protected MinorWhiteInternalThreat(int health, int speed, StationLocation currentStation, PlayerAction actionType, int? accessibility = null) :
			base(ThreatDifficulty.White, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected MinorWhiteInternalThreat(int health, int speed, List<StationLocation> currentStations, PlayerAction actionType, int? accessibility = null) :
			base(ThreatDifficulty.White, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
