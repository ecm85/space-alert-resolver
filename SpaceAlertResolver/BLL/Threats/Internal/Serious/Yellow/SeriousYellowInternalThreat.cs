using System.Collections.Generic;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public abstract class SeriousYellowInternalThreat : SeriousInternalThreat
	{
		protected SeriousYellowInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType)
			: base(ThreatDifficulty.Yellow, health, speed, currentStation, actionType)
		{
		}

		protected SeriousYellowInternalThreat(int health, int speed, IList<StationLocation> currentStations, PlayerActionType actionType)
			: base(ThreatDifficulty.Yellow, health, speed, currentStations, actionType)
		{
		}

		protected SeriousYellowInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility)
			: base(ThreatDifficulty.Yellow, health, speed, currentStation, actionType, accessibility)
		{
		}
	}
}
