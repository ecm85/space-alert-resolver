using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Common;

namespace BLL.Threats.External.Minor.White
{
	public class Amoeba : MinorWhiteExternalThreat
	{
		public Amoeba()
			: base(0, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Repair(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Repair(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(5);
		}
		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
