using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Common;

namespace BLL.Threats.External.Minor.White
{
	public class Meteoroid : MinorWhiteExternalThreat
	{
		public Meteoroid()
			: base(0, 5, 5)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(RemainingHealth);
		}
		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
