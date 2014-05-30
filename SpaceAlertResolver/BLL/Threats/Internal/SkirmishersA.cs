using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SkirmishersA : MinorWhiteInternalThreat
	{
		protected SkirmishersA(int timeAppears, SittingDuck sittingDuck)
			: base(0, 1, 3, timeAppears, sittingDuck.RedZone.UpperStation, PlayerAction.BattleBots)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			CurrentStation = CurrentStation.RedwardStation;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			CurrentStation = CurrentStation.OppositeDeckStation;
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(3, CurrentStation.ZoneLocation);
		}
	}
}
