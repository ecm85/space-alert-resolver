using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Swarm : MinorYellowExternalThreat
	{
		public Swarm(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(0, 3, 2, timeAppears, currentZone, sittingDuck)
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

		public static string GetDisplayName()
		{
			return "Swarm";
		}
	}
}
