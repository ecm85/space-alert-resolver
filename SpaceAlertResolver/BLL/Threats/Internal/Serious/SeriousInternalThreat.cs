using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious
{
	public abstract class SeriousInternalThreat : InternalThreat
	{
		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, timeAppears, currentStation, actionType, sittingDuck, accessibility)
		{
		}

		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, int timeAppears, List<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null)
			: base((ThreatType)ThreatType.SeriousInternal, difficulty, health, speed, timeAppears, currentStations, actionType, sittingDuck, inaccessibility: accessibility)
		{
		}
	}
}
