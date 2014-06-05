using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.MinorExternal.Yellow
{
	public class Jellyfish : MinorYellowExternalThreat
	{
		public Jellyfish(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(-2, 13, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			AttackAllZones(1);
			HealHalfDamage();
		}

		public override void PerformYAction()
		{
			AttackAllZones(1);
			HealHalfDamage();
		}

		public override void PerformZAction()
		{
			AttackAllZones(2);
		}

		private void HealHalfDamage()
		{
			RemainingHealth += (TotalHealth - RemainingHealth) / 2;
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		public static string GetDisplayName()
		{
			return "Jellyfish";
		}
	}
}
