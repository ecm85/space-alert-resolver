using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor
{
	public abstract class MinorInternalThreat : InternalThreat
	{
		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null) :
			base(ThreatType.MinorExternal, difficulty, health, speed, timeAppears, currentStation, actionType, sittingDuck, accessibility)
		{
		}

		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, List<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null) :
			base((ThreatType)ThreatType.MinorExternal, difficulty, health, speed, timeAppears, currentStations, actionType, sittingDuck, inaccessibility: accessibility)
		{
		}
	}
}
