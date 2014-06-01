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
			var playersWithBattleBots = sittingDuck.Zones.SelectMany(zone => zone.Players).Where(player => player.BattleBots != null);
			KnockOut(playersWithBattleBots);
		}

		public override void PerformYAction()
		{
			var playersInCurrentStations = CurrentStations.SelectMany(station => station.Players);
			KnockOut(playersInCurrentStations);
		}

		public override void PerformZAction()
		{
			var playersNotOnBridge = sittingDuck.Zones.SelectMany(zone => zone.Players).Except(sittingDuck.WhiteZone.UpperStation.Players);
			KnockOut(playersNotOnBridge);
		}

		//TODO: Extra 1 damage when hit by in both zones
	}
}
