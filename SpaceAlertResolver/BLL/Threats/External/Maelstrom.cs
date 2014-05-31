using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Maelstrom : SeriousWhiteExternalThreat
	{
		public Maelstrom(int timeAppears, Zone currentZone)
			: base(3, 8, 2, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			sittingDuck.DrainAllShields();
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			AttackOtherTwoZones(2, sittingDuck);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			AttackOtherTwoZones(3, sittingDuck);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var hitByPulse = damages.Any(damage => damage.DamageType == DamageType.Pulse);
			if (hitByPulse)
				remainingHealth -= damages.Sum(damage => damage.Amount);
			else
				base.TakeDamage(damages);
		}
	}
}
