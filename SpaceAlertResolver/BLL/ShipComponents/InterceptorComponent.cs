using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class InterceptorComponent : CComponent
	{
		public override CResult PerformCAction(Player performingPlayer)
		{
			if (performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled)
				return new CResult
				{
					TakeOffInInterceptors = true
				};
			return new CResult();
		}
	}
}
