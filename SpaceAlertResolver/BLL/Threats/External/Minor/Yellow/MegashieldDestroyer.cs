using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MegashieldDestroyer : MinorYellowExternalThreat
	{
		public MegashieldDestroyer(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(4, 3, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override void PerformYAction()
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override void PerformZAction()
		{
			Attack(3, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any(damage => damage.Amount > 0) && shields > 0)
				shields--;
		}

		public static string GetDisplayName()
		{
			return "Megashield Destroyer";
		}
	}
}
