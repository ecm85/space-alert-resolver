using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class UnstableWarheads : MinorWhiteInternalThreat
	{
		public UnstableWarheads(int timeAppears, SittingDuck sittingDuck)
			: base(sittingDuck.RocketsComponent.Rockets.Count, 3, timeAppears, sittingDuck.BlueZone.LowerStation, PlayerAction.C, sittingDuck)
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
			Damage(RemainingHealth * 3);
		}

		public override string GetDisplayName()
		{
			return "Unstable Warheads";
		}
	}
}
