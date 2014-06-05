using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MinorAsteroid : MinorYellowExternalThreat
	{
		private int breakpointsCrossed;

		public MinorAsteroid(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(0, 7, 4, timeAppears, currentZone, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Minor Asteroid";
		}

		public override void PeformXAction()
		{
			breakpointsCrossed++;
		}

		public override void PerformYAction()
		{
			breakpointsCrossed++;
		}

		public override void PerformZAction()
		{
			Attack(RemainingHealth);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			Attack(1 * breakpointsCrossed);
		}
	}
}
