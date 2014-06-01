using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class ComputerComponent : CComponent
	{
		public override void PerformCAction(Player performingPlayer)
		{
			MaintenancePerformedThisPhase = true;
		}

		public bool MaintenancePerformedThisPhase { get; private set; }

		public void PerformEndOfPhase()
		{
			MaintenancePerformedThisPhase = false;
		}
	}
}
