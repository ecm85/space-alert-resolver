using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class CrossedWires : SeriousWhiteInternalThreat
	{
		public CrossedWires(int timeAppears, SittingDuck sittingDuck)
			: base(4, 3, timeAppears, sittingDuck.WhiteZone.UpperStation, PlayerAction.B, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			var shield = CurrentStation.EnergyContainer;
			var reactor = CurrentStation.OppositeDeckStation.EnergyContainer;
			var roomForShields = shield.Capacity - shield.Energy;
			var energyTransferredToShields = Math.Min(roomForShields, reactor.Energy);
			shield.Energy += energyTransferredToShields;
			reactor.Energy -= energyTransferredToShields;
			sittingDuck.TakeDamage(reactor.Energy, CurrentStation.ZoneLocation);
			reactor.Energy = 0;
		}

		public override void PerformYAction()
		{
			var reactor = CurrentStation.OppositeDeckStation.EnergyContainer;
			sittingDuck.TakeDamage(reactor.Energy, CurrentStation.ZoneLocation);
			reactor.Energy = 0;
		}

		public override void PerformZAction()
		{
			foreach (var zone in sittingDuck.Zones)
			{
				var reactor = zone.LowerStation.EnergyContainer;
				sittingDuck.TakeDamage(reactor.Energy, zone.ZoneLocation);
				reactor.Energy = 0;
			}
		}
	}
}
