using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Jellyfish : MinorYellowExternalThreat
	{
		public Jellyfish()
			: base(-2, 13, 2)
		{
		}

		public override void PerformXAction()
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
