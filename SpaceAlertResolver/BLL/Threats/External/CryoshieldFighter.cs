using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class CryoshieldFighter : MinorWhiteExternalThreat
	{
		public CryoshieldFighter(int timeAppears, Zone currentZone)
			: base(1, 4, 3, timeAppears, currentZone)
		{
		}

		private bool shielded = true;

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(1, CurrentZone);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(2, CurrentZone);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(2, CurrentZone);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (shielded && damages.Any())
				shielded = false;
			else
				base.TakeDamage(damages);
		}
	}
}
