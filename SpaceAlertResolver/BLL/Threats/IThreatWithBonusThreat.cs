namespace BLL.Threats
{
    public interface IThreatWithBonusThreat<T> where T: Threat
    {
        T BonusThreat { get; set; }
    }
}
