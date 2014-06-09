using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class HackedShields : MinorWhiteInternalThreat
	{
		protected HackedShields(StationLocation station)
			: base(3, 2, station, PlayerAction.B)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShields(new [] {CurrentZone});
		}

		public override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainReactors(new [] {CurrentZone});
		}

		public override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}
	}
}
