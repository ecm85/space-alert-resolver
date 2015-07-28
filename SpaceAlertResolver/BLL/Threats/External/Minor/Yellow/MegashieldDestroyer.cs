using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MegashieldDestroyer : MinorYellowExternalThreat
	{
		public MegashieldDestroyer()
			: base(4, 3, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(3, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any() && Shields > 0)
				Shields--;
		}
	}
}
