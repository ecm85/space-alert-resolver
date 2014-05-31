using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class PlasmaticFighter : MinorWhiteExternalThreat
	{
		public PlasmaticFighter(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(2, 4, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(1);
		}

		public override void PerformYAction()
		{
			Attack(1);
		}

		public override void PerformZAction()
		{
			Attack(2);
		}

		protected override ExternalPlayerDamageResult Attack(int amount)
		{
			var result = base.Attack(amount);
			if (result.DamageShielded == 0)
				//TODO: Knock out all players in current zone
				throw new NotImplementedException();
			return result;
		}
	}
}
