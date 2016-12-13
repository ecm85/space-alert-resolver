namespace BLL.Threats
{
	public interface IThreatWithBonusThreat<in T> where T: Threat
	{
		void SetBonusThreat(T bonusThreat);
	}
}
