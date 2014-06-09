using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public abstract class SeriousYellowInternalThreat : SeriousInternalThreat
	{
		protected SeriousYellowInternalThreat(int health, int speed, StationLocation currentStation, PlayerAction actionType, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected SeriousYellowInternalThreat(int health, int speed, List<StationLocation> currentStations, PlayerAction actionType, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
