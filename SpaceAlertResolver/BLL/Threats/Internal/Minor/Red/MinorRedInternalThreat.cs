using System.Collections.Generic;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
    public abstract class MinorRedInternalThreat : MinorInternalThreat
    {
        protected MinorRedInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType) :
            base(ThreatDifficulty.Red, health, speed, currentStation, actionType)
        {
        }

        protected MinorRedInternalThreat(int health, int speed, IList<StationLocation> currentStations, PlayerActionType actionType) :
            base(ThreatDifficulty.Red, health, speed, currentStations, actionType)
        {
        }

        protected MinorRedInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType, int? accessibility) :
            base(ThreatDifficulty.Red, health, speed, currentStation, actionType, accessibility)
        {
        }
    }
}
