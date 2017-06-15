using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
    public abstract class MinorWhiteInternalThreat : MinorInternalThreat
    {
        protected MinorWhiteInternalThreat(int health, int speed, StationLocation currentStation, PlayerActionType actionType) :
            base(ThreatDifficulty.White, health, speed, currentStation, actionType)
        {
        }
    }
}
