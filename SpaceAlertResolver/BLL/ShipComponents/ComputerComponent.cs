using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class ComputerComponent : CComponent
	{
		public override CResult PerformCAction(Player performingPlayer)
		{
			MaintenancePerformedThisPhase = true;
			return new CResult();
		}

		public bool MaintenancePerformedThisPhase { get; private set; }

		public void PerformEndOfPhase()
		{
			MaintenancePerformedThisPhase = false;
		}
	}
}
