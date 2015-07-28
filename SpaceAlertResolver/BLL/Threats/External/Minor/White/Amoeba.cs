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
			Attack(5);
		}
		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
