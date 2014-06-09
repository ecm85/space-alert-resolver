using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class Meteoroid : MinorWhiteExternalThreat
	{
		public Meteoroid()
			: base(0, 5, 5)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
		}

		public override void PerformYAction(int currentTurn)
		{
		}

		public override void PerformZAction(int currentTurn)
		{
			Attack(RemainingHealth);
		}

		public static string GetDisplayName()
		{
			return "Meteoroid";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
