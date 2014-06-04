using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class Maelstrom : SeriousWhiteExternalThreat
	{
		public Maelstrom(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(3, 8, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.DrainShields(EnumFactory.All<ZoneLocation>());
		}

		public override void PerformYAction()
		{
			AttackOtherTwoZones(2);
		}

		public override void PerformZAction()
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
				var oldShields = Shields;
				Shields = 0;
				base.TakeDamage(damages);
				Shields = oldShields;
			}
			else
				base.TakeDamage(damages);
		}
	}
}
