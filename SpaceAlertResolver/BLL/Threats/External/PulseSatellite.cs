using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class PulseSatellite : SeriousWhiteExternalThreat
	{
		public PulseSatellite(int shields, int health, int speed, int timeAppears, Zone currentZone)
			: base(shields, health, speed, timeAppears, currentZone)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			AttackAllZones(1, sittingDuck);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			AttackAllZones(2, sittingDuck);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			AttackAllZones(3, sittingDuck);
		}

		//TODO: Cannot be targeted at distance 3
	}
}
