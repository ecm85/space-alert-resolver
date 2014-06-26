using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public abstract class MinorRedInternalThreat : MinorInternalThreat
	{
		protected MinorRedInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility = null) :
			base(ThreatDifficulty.Red, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected MinorRedInternalThreat(int health, int speed, List<StationLocation> currentStations, PlayerActionType actionType, int? accessibility = null) :
			base(ThreatDifficulty.Red, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
