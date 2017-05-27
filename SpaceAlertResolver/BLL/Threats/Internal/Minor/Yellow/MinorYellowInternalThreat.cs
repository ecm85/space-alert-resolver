using System.Collections.Generic;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class MinorYellowInternalThreat : MinorInternalThreat
	{
		protected MinorYellowInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType)
			: base(ThreatDifficulty.Yellow, health, speed, currentStation, actionType)
		{
		}

		protected MinorYellowInternalThreat(int health, int speed, IList<StationLocation> currentStations, PlayerActionType actionType)
			: base(ThreatDifficulty.Yellow, health, speed, currentStations, actionType)
		{
		}
	}
}
