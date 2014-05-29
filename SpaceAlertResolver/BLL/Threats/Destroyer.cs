using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public class Destroyer : MinorExternalThreat
	{
		public Destroyer(int timeAppears, Zone currentZone)
			: base(2, 4, 2, 5, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			DealDoubleDamageThroughShields(sittingDuck, CurrentZone, 1);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			DealDoubleDamageThroughShields(sittingDuck, CurrentZone, 2);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			DealDoubleDamageThroughShields(sittingDuck, CurrentZone, 2);
		}

		private static void DealDoubleDamageThroughShields(SittingDuck sittingDuck, Zone currentZone, int amount)
		{
			var damageResult = sittingDuck.TakeDamage(amount, currentZone);
			if (damageResult.DamageDone > 0)
				sittingDuck.TakeDamage(damageResult.DamageDone, currentZone);
		}
	}
}
