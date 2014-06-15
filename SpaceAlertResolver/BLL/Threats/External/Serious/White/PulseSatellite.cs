using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class PulseSatellite : SeriousWhiteExternalThreat
	{
		public PulseSatellite()
			: base(2, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackAllZones(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackAllZones(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(3);
		}

		public static string GetDisplayName()
		{
			return "Pulse Satellite";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return DistanceToShip != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
