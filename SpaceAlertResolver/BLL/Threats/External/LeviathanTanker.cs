using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class LeviathanTanker : SeriousWhiteExternalThreat
	{
		public LeviathanTanker(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(3, 8, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			Attack(2);
			Repair(2);
		}

		public override void PerformZAction()
		{
			Attack(2);
		}

		protected override void OnDestroyed()
		{
			foreach (var threat in sittingDuck.CurrentExternalThreats)
			{
				threat.RemainingHealth -= 1;
				threat.CheckForDestroyed();
			}
		}
	}
}
