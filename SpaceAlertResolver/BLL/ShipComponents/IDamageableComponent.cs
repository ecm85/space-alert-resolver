namespace BLL.ShipComponents
{
	public interface IDamageableComponent
	{
		void SetDamaged(bool isCampaignDamage);
		void Repair();
	}
}
