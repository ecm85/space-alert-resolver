namespace BLL.ShipComponents
{
    public class BatteryPack : IEnergyProvider
    {
        public bool HasEnergy { get; private set; } = true;

        public void UseEnergy(int amount)
        {
            HasEnergy = false;
        }

        public bool CanUseEnergy(int amount)
        {
            return amount == 1 && HasEnergy;
        }

        public EnergyType EnergyType { get; } = EnergyType.Battery;
        public void PerformEndOfTurn()
        {
            HasEnergy = true;
        }
    }
}
