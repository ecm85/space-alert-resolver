using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class CryoshieldFrigate : SeriousWhiteExternalThreat
	{
		private bool cryoshieldUp = true;

		public CryoshieldFrigate(int timeAppears, Zone currentZone)
			: base(1, 7, 3, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			Attack(3);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(4);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			//TODO: Rules clarification: Does damage have to get through regular shield to take down cryoshield?
			if (cryoshieldUp && damages.Any())
				cryoshieldUp = false;
			else
				base.TakeDamage(damages);
		}
	}
}
