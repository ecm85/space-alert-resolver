using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class EnergyCloud : MinorWhiteExternalThreat
	{
		public EnergyCloud()
			: base(3, 5, 2)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>());
		}

		public override void PerformYAction(int currentTurn)
		{
			AttackOtherTwoZones(1);
		}

		public override void PerformZAction(int currentTurn)
		{
			AttackOtherTwoZones(2);
		}

		public static string GetDisplayName()
		{
			return "Energy Cloud";
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
