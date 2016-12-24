namespace BLL.ShipComponents
{
	public class BatteryPack : IEnergyProvider
	{
		public int Energy { get; private set; } = 1;

		public void UseEnergy(int amount)
		{
			Energy = 0;
		}

		public bool CanUseEnergy(int amount)
		{
			return amount == 1 && Energy == 1;
		}

		public EnergyType EnergyType { get; } = EnergyType.Battery;
		public void PerformEndOfTurn()
		{
			Energy = 1;
		}
	}
}
