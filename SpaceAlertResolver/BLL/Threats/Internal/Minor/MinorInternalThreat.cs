using System.Collections.Generic;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor
{
	public abstract class MinorInternalThreat : InternalThreat
	{
		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerActionType actionType) :
			base(ThreatType.MinorInternal, difficulty, health, speed, currentStation, actionType)
		{
		}

		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, IList<StationLocation> currentStations, PlayerActionType actionType) :
			base(ThreatType.MinorInternal, difficulty, health, speed, currentStations, actionType)
		{
		}

		protected MinorInternalThreat(ThreatDifficulty difficulty, int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility) :
			base(ThreatType.MinorInternal, difficulty, health, speed, currentStation, actionType, accessibility)
		{
		}
	}
}
