namespace BLL.Threats.External.Minor.Red
{
    public abstract class MinorRedExternalThreat : MinorExternalThreat
    {
        protected MinorRedExternalThreat(int shields, int health, int speed) :
            base(ThreatDifficulty.Red, shields, health, speed)
        {
        }
    }
}
