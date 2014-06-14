using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.White
{
	public class Maelstrom : SeriousWhiteExternalThreat
	{
		public Maelstrom()
			: base(3, 8, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>());
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackOtherTwoZones(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackOtherTwoZones(3);
		}

		public static string GetDisplayName()
		{
			return "Maelstrom";
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var hitByPulse = damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Pulse);
			if (hitByPulse)
			{
				var oldShields = shields;
				shields = 0;
				base.TakeDamage(damages);
				shields = oldShields;
			}
			else
				base.TakeDamage(damages);
		}
	}
}
