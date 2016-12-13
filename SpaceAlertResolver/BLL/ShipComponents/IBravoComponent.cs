namespace BLL.ShipComponents
{
	public interface IBravoComponent : IDamageableComponent
	{
		void PerformBAction(bool isHeroic);
	}
}
