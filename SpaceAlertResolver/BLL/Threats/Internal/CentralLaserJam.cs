using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class CentralLaserJam : MinorWhiteInternalThreat
	{
		public CentralLaserJam(int timeAppears, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, sittingDuck.WhiteZone.UpperStation, PlayerAction.A, sittingDuck)
		{
		}

		//TODO: Final repair action requires and uses an energy from central reactor

		public override void PeformXAction()
		{
			CurrentStation.OppositeDeckStation.EnergyContainer.Energy -= 1;
		}

		public override void PerformYAction()
		{
			sittingDuck.TakeDamage(1, CurrentStation.ZoneLocation);
		}

		public override void PerformZAction()
		{
			sittingDuck.TakeDamage(3, CurrentStation.ZoneLocation);
			sittingDuck.TakeDamage(1, EnumFactory.All<ZoneLocation>().Except(new [] {CurrentStation.ZoneLocation}));
		}
	}
}
