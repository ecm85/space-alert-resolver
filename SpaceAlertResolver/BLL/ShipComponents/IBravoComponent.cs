namespace BLL.ShipComponents
{
	public interface IBravoComponent : IDamageableComponent, IEnergyProvider
	{
		void PerformBAction(bool isHeroic);
		int EnergyInComponent { get; }
		int Capacity { get; }
	}
}
