using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class MinorYellowInternalThreat : MinorInternalThreat
	{
		protected MinorYellowInternalThreat(int health, int speed, StationLocation currentStation, PlayerAction actionType, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected MinorYellowInternalThreat(int health, int speed, List<StationLocation> currentStations, PlayerAction actionType, int? accessibility = null)
			: base(ThreatDifficulty.Yellow, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
