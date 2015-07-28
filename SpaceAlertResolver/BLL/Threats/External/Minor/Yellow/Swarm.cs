using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Swarm : MinorYellowExternalThreat
	{
		public Swarm()
			: base(0, 3, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
			AttackOtherTwoZones(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(3);
			AttackOtherTwoZones(2);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			TakeDamage(damages, 1);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
