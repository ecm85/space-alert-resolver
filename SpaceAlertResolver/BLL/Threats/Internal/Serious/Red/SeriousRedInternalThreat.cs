using System.Collections.Generic;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public abstract class SeriousRedInternalThreat : SeriousInternalThreat
	{
		protected SeriousRedInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType) :
			base(ThreatDifficulty.Red, health, speed, currentStation, actionType)
		{
		}

		protected SeriousRedInternalThreat(int health, int speed, IList<StationLocation> currentStations, PlayerActionType actionType) :
			base(ThreatDifficulty.Red, health, speed, currentStations, actionType)
		{
		}

		protected SeriousRedInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility = null) :
			base(ThreatDifficulty.Red, health, speed, currentStation, actionType, accessibility)
		{
		}
	}
}
