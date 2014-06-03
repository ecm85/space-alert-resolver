﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Meteoroid : MinorWhiteExternalThreat
	{
		public Meteoroid(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(0, 5, 5, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
		}

		public override void PerformYAction()
		{
		}

		public override void PerformZAction()
		{
			Attack(RemainingHealth);
		}

		public override string GetDisplayName()
		{
			return "Meteoroid";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.DamageType != DamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
