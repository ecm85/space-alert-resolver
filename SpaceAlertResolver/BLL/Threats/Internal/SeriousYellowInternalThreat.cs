using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class SeriousYellowInternalThreat : SeriousInternalThreat
	{
		protected SeriousYellowInternalThreat(int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck)
			: base(ThreatDifficulty.Yellow, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		protected SeriousYellowInternalThreat(int health, int speed, int timeAppears, IList<Station> currentStations, PlayerAction actionType, SittingDuck sittingDuck)
			: base(ThreatDifficulty.Yellow, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}
	}
}
