namespace BLL.ShipComponents
{
	public interface IAlphaComponent : IDamageableComponent
	{
		void PerformAAction(bool isHeroic, Player performingPlayer, bool isAdvanced);
	}
}
