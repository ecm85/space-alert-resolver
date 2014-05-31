using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class BattleBotUprising : SeriousWhiteInternalThreat
	{
		public BattleBotUprising(int timeAppears, SittingDuck sittingDuck)
			: base(4, 2, timeAppears, new []{sittingDuck.BlueZone.UpperStation, sittingDuck.RedZone.LowerStation}, PlayerAction.C, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			//TODO: Knock out all players with an active battlebot squad (regardless of location)
		}

		public override void PerformYAction()
		{
			//TODO: Knock out all players in currentStations
		}

		public override void PerformZAction()
		{
			//TODO: Knock out all players not on the bridge
		}

		//TODO: Extra 1 damage when hit by in both zones
	}
}
