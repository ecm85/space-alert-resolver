using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Destroyer : MinorWhiteExternalThreat
	{
		public Destroyer(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(2, 5, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(1);
		}

		public override void PerformYAction()
		{
			Attack(2);
		}

		public override void PerformZAction()
		{
			Attack(2);
		}

		public override string GetDisplayName()
		{
			return "Destroyer";
		}

		protected override ExternalThreatDamageResult Attack(int amount)
		{
			var damageResult = base.Attack(amount);
			if (damageResult.DamageDone > 0)
				damageResult.AddDamage(base.Attack(damageResult.DamageDone));
			return damageResult;
		}
	}
}
