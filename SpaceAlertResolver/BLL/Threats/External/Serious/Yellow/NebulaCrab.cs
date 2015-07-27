using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Yellow
{
	public class NebulaCrab : SeriousYellowExternalThreat
	{
		public NebulaCrab()
			: base(2, 7, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			shields = 4;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed += 2;
			shields = 2;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackSpecificZones(5, new[] {ZoneLocation.Red, ZoneLocation.Blue});
		}

		public static string GetDisplayName()
		{
			return "Nebula Crab";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		public static string GetId()
		{
			return "SE2-04";
		}
	}
}
