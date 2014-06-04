using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class StealthFighter : MinorWhiteExternalThreat
	{
		private bool stealthed = true;

		public StealthFighter(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(2, 4, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			stealthed = false;
		}

		public override void PerformYAction()
		{
			Attack(2);
		}

		public override void PerformZAction()
		{
			Attack(2);
		}

		public static string GetDisplayName()
		{
			return "Stealth Fighter";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return !stealthed && base.CanBeTargetedBy(damage);
		}
	}
}
