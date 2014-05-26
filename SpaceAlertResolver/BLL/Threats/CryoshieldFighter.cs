using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public class CryoshieldFighter : MinorExternalThreat
	{
		public CryoshieldFighter(int timeAppears, ZoneType currentZoneType)
			: base(2, 4, 1, 4, 3, timeAppears, currentZoneType)
		{
		}

		private bool shielded = true;

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(1, CurrentZoneType);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(2, CurrentZoneType);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(2, CurrentZoneType);
		}

		public override void TakeDamage(IList<Damage> damages)
		{
			if (shielded && damages.Any())
				shielded = false;
			else
				base.TakeDamage(damages);
		}
	}
}
