using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public abstract class SeriousWhiteInternalThreat : SeriousInternalThreat
	{
		protected SeriousWhiteInternalThreat(int health, int speed, StationLocation currentStation, PlayerAction actionType, int? accessibility = null) :
			base(ThreatDifficulty.White, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected SeriousWhiteInternalThreat(int health, int speed, List<StationLocation> currentStations, PlayerAction actionType, int? accessibility = null) :
			base(ThreatDifficulty.White, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
