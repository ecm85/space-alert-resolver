using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public interface ICharlieComponent
	{
		void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage);
		bool CanPerformCAction(Player performingPlayer);
	}
}
