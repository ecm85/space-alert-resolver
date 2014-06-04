using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class EnergyCloud : MinorWhiteExternalThreat
	{
		public EnergyCloud(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(3, 5, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.DrainShields(EnumFactory.All<ZoneLocation>());
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
			var hitByPulse = damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Pulse);
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
