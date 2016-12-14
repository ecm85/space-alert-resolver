namespace BLL.ShipComponents
{
	public abstract class Reactor : EnergyContainer, IBravoComponent
	{
		protected Reactor(int capacity, int energy) : base(capacity, energy)
		{
		}

		public abstract void PerformBAction(bool isHeroic);

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
