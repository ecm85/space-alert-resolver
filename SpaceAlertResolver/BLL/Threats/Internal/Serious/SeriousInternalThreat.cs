using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious
{
	public abstract class SeriousInternalThreat : InternalThreat
	{
		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
		}

		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, IList<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, timeAppears, currentStations, actionType, sittingDuck)
		{
		}
	}
}
