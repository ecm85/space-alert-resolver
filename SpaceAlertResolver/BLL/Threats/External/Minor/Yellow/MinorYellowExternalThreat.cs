namespace BLL.Threats.External.Minor.Yellow
{
    public abstract class MinorYellowExternalThreat : MinorExternalThreat
    {
        protected MinorYellowExternalThreat(int shields, int health, int speed)
            : base(ThreatDifficulty.Yellow, shields, health, speed)
        {
        }
    }
}
