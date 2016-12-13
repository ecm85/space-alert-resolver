namespace BLL.Threats.External.Serious
{
	public abstract class SeriousExternalThreat : ExternalThreat
	{
		protected SeriousExternalThreat(ThreatDifficulty difficulty, int shields, int health, int speed)
			: base(ThreatType.SeriousExternal, difficulty, shields, health, speed)
		{
		}
	}
}
