using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public class EnergyCloud : MinorExternalThreat
	{
		public EnergyCloud(int timeAppears, Zone currentZone)
			: base(2, 4, 3, 5, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			sittingDuck.DrainAllShields();
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			AttackOtherTwoZones(sittingDuck, CurrentZone, 1);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			AttackOtherTwoZones(sittingDuck, CurrentZone, 2);
		}

		private static void AttackOtherTwoZones(SittingDuck sittingDuck, Zone currentZone, int amount)
		{
			sittingDuck.TakeDamage(amount, sittingDuck.Zones.Except(new[] { currentZone }).ToArray());
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var hitByPulse = damages.Any(damage => damage.DamageType == DamageType.Pulse);
			if (hitByPulse)
			{
				var oldShields = shields;
				shields = 0;
				base.TakeDamage(damages);
				shields = oldShields;
			}
			else
				base.TakeDamage(damages);
		}
	}
}
