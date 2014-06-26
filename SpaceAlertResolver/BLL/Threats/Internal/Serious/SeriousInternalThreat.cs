using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious
{
	public abstract class SeriousInternalThreat : InternalThreat
	{
		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility = null)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, currentStation, actionType, accessibility)
		{
		}

		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, List<StationLocation> currentStations, PlayerActionType actionType, int? accessibility = null)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, currentStations, actionType, accessibility)
		{
		}
	}
}
