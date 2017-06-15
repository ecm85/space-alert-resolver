namespace BLL.Threats.External.Minor.White
{
    public abstract class MinorWhiteExternalThreat : MinorExternalThreat
    {
        protected MinorWhiteExternalThreat(int shields, int health, int speed) :
            base(ThreatDifficulty.White, shields, health, speed)
        {
        }
    }
}
