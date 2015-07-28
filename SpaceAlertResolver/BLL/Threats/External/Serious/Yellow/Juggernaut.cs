using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Juggernaut : SeriousYellowExternalThreat
	{
		public Juggernaut()
			: base(3, 10, 1)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			Speed += 2;
			Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed += 2;
			Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(7);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Rocket))
				Shields++;
		}

		public override bool IsPriorityTargetFor(PlayerDamage damage)
		{
			return damage.PlayerDamageType == PlayerDamageType.Rocket;
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return base.CanBeTargetedBy(damage) || damage.PlayerDamageType == PlayerDamageType.Rocket;
		}
	}
}
