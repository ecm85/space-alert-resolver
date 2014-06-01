using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class VisualConfirmationComponent : CComponent
	{
		public override CResult PerformCAction(Player performingPlayer)
		{
			return new CResult
			{
				VisualConfirmationPerformed = true
			};
		}
	}
}
