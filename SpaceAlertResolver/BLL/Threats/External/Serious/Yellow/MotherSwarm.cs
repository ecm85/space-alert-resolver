using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class MotherSwarm : SeriousYellowExternalThreat
	{
		public MotherSwarm(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(1, 6, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(1);
		}

		public override void PerformYAction()
		{
			Attack(2);
			AttackOtherTwoZones(1);
		}

		public override void PerformZAction()
		{
			Attack(4);
			AttackOtherTwoZones(3);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			TakeDamage(damages, 2);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		public static string GetDisplayName()
		{
			return "Mother Swarm";
		}
	}
}
