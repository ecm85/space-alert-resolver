using System.Collections.Generic;

namespace BLL.ShipComponents
{
	public class ComputerComponent : ICharlieComponent
	{
		public int MaintenanceChecksRemaining { get; set; } = 3;

		public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			MaintenancePerformedThisPhase = true;
			MaintenanceChecksRemaining--;
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

		public void PerformComputerCheck(IEnumerable<Player> players, int currentTurn)
		{
			if (!MaintenancePerformedThisPhase)
			{
				foreach (var player in players)
					player.Shift(currentTurn + 1);
				MaintenanceChecksRemaining--;
			}
		}
	}
}
