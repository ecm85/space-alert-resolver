using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class EnergyCloud : MinorWhiteExternalThreat
	{
		public EnergyCloud(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(3, 5, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.DrainAllShields();
		}

		public override void PerformYAction()
		{
			AttackOtherTwoZones(1);
		}

		public override void PerformZAction()
		{
			AttackOtherTwoZones(2);
		}

		public static string GetDisplayName()
		{
			return "Energy Cloud";
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
