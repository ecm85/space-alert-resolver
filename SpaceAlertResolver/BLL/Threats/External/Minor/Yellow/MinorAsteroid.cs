using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MinorAsteroid : MinorYellowExternalThreat
	{
		private int breakpointsCrossed;

		public MinorAsteroid()
			: base(0, 7, 4)
		{
		}

		public static string GetDisplayName()
		{
			return "Minor Asteroid";
		}

		public override void PerformXAction()
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

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			Attack(1 * breakpointsCrossed);
		}
	}
}
