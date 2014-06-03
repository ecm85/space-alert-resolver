using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class PhantomFighter : MinorYellowExternalThreat
	{
		private bool phantomMode = true;

		public PhantomFighter(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(3, 3, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			phantomMode = false;
		}

		public override void PerformYAction()
		{
			Attack(2);
		}

		public override void PerformZAction()
		{
			Attack(3);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !phantomMode && base.CanBeTargetedBy(damage);
		}
	}
}
