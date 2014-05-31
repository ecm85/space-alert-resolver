using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class HackedShieldsB : MinorWhiteInternalThreat
	{
		public HackedShieldsB(int timeAppears, SittingDuck sittingDuck)
			: base(3, 2, timeAppears, sittingDuck.BlueZone.UpperStation, PlayerAction.B, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			CurrentStation.EnergyContainer.Energy = 0;
		}

		public override void PerformYAction()
		{
			CurrentStation.OppositeDeckStation.EnergyContainer.Energy = 0;
		}

		public override void PerformZAction()
		{
			sittingDuck.TakeDamage(2, CurrentStation.ZoneLocation);
		}
	}
}
