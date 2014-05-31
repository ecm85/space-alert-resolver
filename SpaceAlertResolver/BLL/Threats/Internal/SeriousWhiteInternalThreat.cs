using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class SeriousWhiteInternalThreat : SeriousInternalThreat
	{
		protected SeriousWhiteInternalThreat(int health, int speed, int timeAppears, Station currentStation, PlayerAction actionType, SittingDuck sittingDuck) :
			base(ThreatDifficulty.White, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		protected SeriousWhiteInternalThreat(int health, int speed, int timeAppears, IList<Station> currentStations, PlayerAction actionType, SittingDuck sittingDuck) :
			base(ThreatDifficulty.White, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}
	}
}
