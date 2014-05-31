﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Maelstrom : SeriousWhiteExternalThreat
	{
		public Maelstrom(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(3, 8, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.DrainAllShields();
		}

		public override void PerformYAction()
		{
			AttackOtherTwoZones(2);
		}

		public override void PerformZAction()
		{
			AttackOtherTwoZones(3);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var hitByPulse = damages.Any(damage => damage.DamageType == DamageType.Pulse);
			if (hitByPulse)
				RemainingHealth -= damages.Sum(damage => damage.Amount);
			else
				base.TakeDamage(damages);
		}
	}
}
