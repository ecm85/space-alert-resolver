using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor
{
	public abstract class MinorInternalThreat : InternalThreat
	{
		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerAction actionType, int? accessibility = null) :
			base(ThreatType.MinorExternal, difficulty, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, List<StationLocation> currentStations, PlayerAction actionType, int? accessibility = null) :
			base(ThreatType.MinorExternal, difficulty, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
