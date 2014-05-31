using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class HackedShieldsA : MinorWhiteInternalThreat
	{
		public HackedShieldsA(int timeAppears, SittingDuck sittingDuck)
			: base(3, 2, timeAppears, sittingDuck.RedZone.UpperStation, PlayerAction.B)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			CurrentStation.EnergyContainer.Energy = 0;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			CurrentStation.OppositeDeckStation.EnergyContainer.Energy = 0;
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(2, CurrentStation.ZoneLocation);
		}
	}
}
