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

		public override void PerformXAction()
		{
			SittingDuck.DrainShields(new [] {CurrentZone});
		}

		public override void PerformYAction()
		{
			SittingDuck.DrainReactors(new [] {CurrentZone});
		}

		public override void PerformZAction()
		{
			Damage(2);
		}
	}
}
