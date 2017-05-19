using System.Collections.Generic;
using System.Linq;

namespace BLL.ShipComponents
{
	public class ComputerComponent : ICharlieComponent
	{
		private bool maintenanceNeededThisPhase = true;
		public IList<int> RemainingComputerCheckTurns { get; } = new List<int>{ 2, 5, 9 };

		public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			if (!RemainingComputerCheckTurns.Any())
				return;
			var nextComputerCheck = RemainingComputerCheckTurns.First();
			if (currentTurn <= nextComputerCheck && maintenanceNeededThisPhase)
			{
				maintenanceNeededThisPhase = false;
				RemainingComputerCheckTurns.Remove(nextComputerCheck);
			}
		}

		public bool CanPerformCAction(Player performingPlayer)
		{
			return maintenanceNeededThisPhase;
		}

		public void PerformEndOfPhase()
		{
			maintenanceNeededThisPhase = true;
		}

		public void PerformComputerCheck(IEnumerable<Player> players, int currentTurn)
		{
			if (maintenanceNeededThisPhase)
			{
				foreach (var player in players)
					player.ShiftAfterPlayerActions(currentTurn);
				maintenanceNeededThisPhase = false;
				RemainingComputerCheckTurns.Remove(currentTurn);
			}
		}

		public bool ShouldCheckComputer(int currentTurn)
		{
			return RemainingComputerCheckTurns.Contains(currentTurn);
		}
	}
}
