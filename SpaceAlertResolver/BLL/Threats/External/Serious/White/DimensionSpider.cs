using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class DimensionSpider : SeriousWhiteExternalThreat
	{
		public DimensionSpider()
			: base(0, 13, 1)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			shields = 1;
		}

		public override void PerformYAction(int currentTurn)
		{
			shields++;
		}

		public override void PerformZAction(int currentTurn)
		{
			AttackAllZones(4);
		}

		public override void OnJumpingToHyperspace()
		{
			if (HasBeenPlaced)
				PerformZAction(-1);
		}

		public static string GetDisplayName()
		{
			return "Dimension Spider";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
