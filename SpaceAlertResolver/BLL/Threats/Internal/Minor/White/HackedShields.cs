using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class HackedShields : MinorWhiteInternalThreat
	{
		protected HackedShields(StationLocation station)
			: base(3, 2, station, PlayerAction.B)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShields(new [] {CurrentZone});
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainReactors(new [] {CurrentZone});
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}
	}
}
