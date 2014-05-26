using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats
{
	public class EnergyCloud : MinorExternalThreat
	{
		public EnergyCloud(int timeAppears, ZoneType currentZoneType)
			: base(2, 4, 3, 5, 2, timeAppears, currentZoneType)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			sittingDuck.DrainAllShields(ZoneTypes.All());
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			AttackOtherTwoZones(sittingDuck, CurrentZoneType, 1);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			AttackOtherTwoZones(sittingDuck, CurrentZoneType, 2);
		}

		private static void AttackOtherTwoZones(SittingDuck sittingDuck, ZoneType currentZoneType, int amount)
		{
			sittingDuck.TakeDamage(amount, ZoneTypes.All().Except(new[] { currentZoneType }).ToArray());
		}

		public override void TakeDamage(IList<Damage> damages)
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
