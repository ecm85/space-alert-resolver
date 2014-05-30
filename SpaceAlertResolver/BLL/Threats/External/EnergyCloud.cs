using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class EnergyCloud : MinorWhiteExternalThreat
	{
		public EnergyCloud(int timeAppears, Zone currentZone)
			: base(3, 5, 2, timeAppears, currentZone)
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
			sittingDuck.TakeAttack(amount, sittingDuck.Zones.Except(new[] { currentZone }).ToArray());
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
