using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class SpacecraftCarrier : SeriousWhiteExternalThreat
	{
		public SpacecraftCarrier(int timeAppears, Zone currentZone, SittingDuck sittingDuck)
			: base(3, 6, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2 - AttackDecrement);
		}

		public override void PerformYAction()
		{
			AttackOtherTwoZones(3 - AttackDecrement);
		}

		public override void PerformZAction()
		{
			AttackAllZones(4 - AttackDecrement);
		}

		public override string GetDisplayName()
		{
			return "Spacecraft Carrier";
		}

		private int AttackDecrement { get { return InterceptorsPresent ? 2 : 0; } }
		private bool InterceptorsPresent { get { return sittingDuck.InterceptorStation.Players.Any(); } }
	}
}
