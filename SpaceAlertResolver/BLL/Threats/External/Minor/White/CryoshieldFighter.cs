﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class CryoshieldFighter : MinorWhiteExternalThreat
	{
		private bool cryoshieldUp = true;

		public CryoshieldFighter()
			: base(1, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}
		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (cryoshieldUp && damages.Any())
				cryoshieldUp = false;
			else
				base.TakeDamage(damages);
		}
	}
}
