using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class MajorAsteroid : SeriousYellowExternalThreat
	{
		private int breakpointsCrossed;

		public MajorAsteroid(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(0, 11, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Major Asteroid";
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
			Attack(3 * breakpointsCrossed);
		}
	}
}
