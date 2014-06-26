using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public abstract class SeriousRedInternalThreat : SeriousInternalThreat
	{
		protected SeriousRedInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility = null) :
			base(ThreatDifficulty.Red, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected SeriousRedInternalThreat(int health, int speed, List<StationLocation> currentStations, PlayerActionType actionType, int? accessibility = null) :
			base(ThreatDifficulty.Red, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
