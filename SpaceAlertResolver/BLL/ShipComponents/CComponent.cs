using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
	public abstract class CComponent
	{
		public void PerformCAction(Player performingPlayer, int currentTurn)
		{
			PerformCAction(performingPlayer, currentTurn, false);
		}
		public abstract void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage);
		public abstract bool CanPerformCAction(Player performingPlayer);
	}
}
