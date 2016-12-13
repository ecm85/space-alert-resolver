namespace BLL.ShipComponents
{
	public interface ICharlieComponent
	{
		void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage);
		bool CanPerformCAction(Player performingPlayer);
	}
}
