using System.Collections.Generic;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious
{
	public abstract class SeriousInternalThreat : InternalThreat
	{
		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerActionType actionType)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, currentStation, actionType)
		{
		}

		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, IList<StationLocation> currentStations, PlayerActionType actionType)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, currentStations, actionType)
		{
		}

		protected SeriousInternalThreat(ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility)
			: base(ThreatType.SeriousInternal, difficulty, health, speed, currentStation, actionType, accessibility)
		{
		}
	}
}
