using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public class SkirmishersA : MinorWhiteInternalThreat
	{
		protected SkirmishersA(int timeAppears, SittingDuck sittingDuck)
			: base(0, 1, 3, timeAppears, sittingDuck.RedZone.UpperStation)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			CurrentStation = CurrentStation.BluewardStation;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			CurrentStation = CurrentStation.OppositeDeckStation;
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			//TODO: make a 'take damage' action and give stations a zone reference
		}
	}
}
