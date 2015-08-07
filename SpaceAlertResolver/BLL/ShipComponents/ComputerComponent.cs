using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class ComputerComponent : CComponent
	{
		public override void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			MaintenancePerformedThisPhase = true;
		}

		public override bool CanPerformCAction(Player performingPlayer)
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
