namespace BLL.Threats.External.Serious.White
{
	public abstract class SeriousWhiteExternalThreat : SeriousExternalThreat
	{
		protected SeriousWhiteExternalThreat(int shields, int health, int speed) :
			base(ThreatDifficulty.White, shields, health, speed)
		{
		}
	}
}
