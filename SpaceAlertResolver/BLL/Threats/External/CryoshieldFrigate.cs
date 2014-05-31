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
			sittingDuck.TakeAttack(2, CurrentZone);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(3, CurrentZone);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(4, CurrentZone);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (!damages.Any())
				return;
			if (cryoshieldUp) //TODO: Rules clarification: Does damage have to get through regular shield to take down cryoshield?
				cryoshieldUp = false;
			else
				base.TakeDamage(damages);
		}
	}
}
