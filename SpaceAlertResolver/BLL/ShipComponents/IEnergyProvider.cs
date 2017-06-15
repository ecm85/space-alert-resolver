namespace BLL.ShipComponents
{
    public interface IEnergyProvider
    {
        void UseEnergy(int amount);
        bool CanUseEnergy(int amount);
        EnergyType EnergyType { get; }
        void PerformEndOfTurn();
    }
}
