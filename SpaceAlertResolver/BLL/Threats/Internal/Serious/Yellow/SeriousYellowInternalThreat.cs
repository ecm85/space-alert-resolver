using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public abstract class SeriousYellowInternalThreat : SeriousInternalThreat
	{
		protected SeriousYellowInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected SeriousYellowInternalThreat(int health, int speed, List<StationLocation> currentStations, PlayerActionType actionType, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
