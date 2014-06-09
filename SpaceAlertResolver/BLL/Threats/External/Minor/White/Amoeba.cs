using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class Amoeba : MinorWhiteExternalThreat
	{
		public Amoeba()
			: base(0, 8, 2)
		{
		}

		public override void PerformXAction()
		{
			Repair(2);
		}

		public override void PerformYAction()
		{
			Repair(2);
		}

		public override void PerformZAction()
		{
			Attack(5);
		}

		public static string GetDisplayName()
		{
			return "Amoeba";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
