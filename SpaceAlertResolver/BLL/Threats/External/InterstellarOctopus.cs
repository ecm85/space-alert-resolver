using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class InterstellarOctopus : SeriousWhiteExternalThreat
	{
		public InterstellarOctopus(int shields, int health, int speed, int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(shields, health, speed, timeAppears, currentZone, sittingDuck)
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
			return damage.DamageType != DamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
