namespace BLL.ShipComponents
{
	public class ComputerComponent : ICharlieComponent
	{
		public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			MaintenancePerformedThisPhase = true;
		}

		public bool CanPerformCAction(Player performingPlayer)
		{
			return true;
		}

		public bool MaintenancePerformedThisPhase { get; private set; }

		public void PerformEndOfPhase()
		{
			MaintenancePerformedThisPhase = false;
		}
	}
}
