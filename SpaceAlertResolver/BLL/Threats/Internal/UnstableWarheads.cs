using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class UnstableWarheads : MinorWhiteInternalThreat
	{
		//TODO: initial Health = remaining warheads
		public UnstableWarheads(int timeAppears, SittingDuck sittingDuck)
			: base(0, 3, timeAppears, sittingDuck.BlueZone.LowerStation, PlayerAction.C, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
		}

		public override void PerformYAction()
		{
		}

		public override void PerformZAction()
		{
			sittingDuck.TakeDamage(RemainingHealth * 3, CurrentStation.ZoneLocation);
		}
	}
}
