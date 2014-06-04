using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class InterstellarOctopus : SeriousWhiteExternalThreat
	{
		public InterstellarOctopus(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(1, 8, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			if (IsDamaged)
				AttackAllZones(1);
		}

		public override void PerformYAction()
		{
			if (IsDamaged)
				AttackAllZones(2);
		}

		public override void PerformZAction()
		{
			Attack(RemainingHealth * 2);
		}

		public static string GetDisplayName()
		{
			return "Interstellar Octopus";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
