namespace BLL.ShipComponents
{
	public class BatteryPack : IEnergyProvider
	{
		//Battery pack recharges so doesn't need to indicate useage and can always be used, but only has 1 capacity.

		public void UseEnergy(int amount)
		{
		}

		public bool CanUseEnergy(int amount)
		{
			return amount == 1;
		}
	}
}
