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
			return new CResult
			{
				ComputerMaintainancePerformed = true
			};
		}
	}
}
