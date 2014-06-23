using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class CComponent
	{
		public abstract void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvanced = false);
		public abstract bool CanPerformCAction(Player performingPlayer);
	}
}
