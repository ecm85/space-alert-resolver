using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Destroyer : MinorWhiteExternalThreat
	{
		public Destroyer(int timeAppears, Zone currentZone)
			: base(2, 5, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			Attack(1);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			Attack(2);
		}

		protected override ExternalPlayerDamageResult Attack(int amount)
		{
			var damageResult = base.Attack(amount);
			if (damageResult.DamageDone > 0)
				damageResult.AddDamage(base.Attack(damageResult.DamageDone));
			return damageResult;
		}
	}
}
