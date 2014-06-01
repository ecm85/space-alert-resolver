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
			MaintenancePerformed = true;
			return new CResult();
		}

		public bool MaintenancePerformed { get; set; }
	}
}
