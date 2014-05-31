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
			sittingDuck.TakeAttack(1, sittingDuck.Zones);
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(2, sittingDuck.Zones);
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeAttack(3, sittingDuck.Zones);
		}
	}
}
