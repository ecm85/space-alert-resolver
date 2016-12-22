namespace BLL.ShipComponents
{
	public abstract class Reactor : EnergyContainer
	{
		protected Reactor(int capacity, int energy) : base(capacity, energy)
		{
		}

		public int Drain(int amount)
		{
			var oldEnergy = Energy;
			Energy -= amount;
			var currentEnergy = Energy;
			return oldEnergy - currentEnergy;
		}

		internal int Empty()
		{
			return Drain(Energy);
		}
	}
}
