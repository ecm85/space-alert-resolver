namespace BLL.ShipComponents
{
	public abstract class Reactor : EnergyContainer
	{
		protected Reactor(int capacity, int energy) : base(capacity, energy)
		{
		}
	}
}
