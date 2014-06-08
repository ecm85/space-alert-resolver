using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class HackedShields : MinorWhiteInternalThreat
	{
		protected HackedShields(int timeAppears, StationLocation station, ISittingDuck sittingDuck)
			: base(3, 2, timeAppears, station, PlayerAction.B, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.DrainShields(new [] {CurrentZone});
		}

		public override void PerformYAction()
		{
			sittingDuck.DrainReactors(new [] {CurrentZone});
		}

		public override void PerformZAction()
		{
			Damage(2);
		}
	}
}
