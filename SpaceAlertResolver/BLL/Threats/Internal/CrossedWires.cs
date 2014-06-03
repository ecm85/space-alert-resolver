using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

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
			TransferEnergyToShield(shield, reactor);
			EnergyLeaksOut(reactor);
		}

		public override void PerformYAction()
		{
			var reactor = CurrentStation.OppositeDeckStation.EnergyContainer;
			EnergyLeaksOut(reactor);
		}

		public override void PerformZAction()
		{
			var allReactors = sittingDuck.Zones.Select(zone => zone.LowerStation.EnergyContainer);
			foreach (var reactor in allReactors)
				EnergyLeaksOut(reactor);
		}

		public override string GetDisplayName()
		{
			return "Crossed Wires";
		}

		private void EnergyLeaksOut(EnergyContainer reactor)
		{
			Damage(reactor.Energy);
			reactor.Energy = 0;
		}

		private static void TransferEnergyToShield(EnergyContainer shield, EnergyContainer reactor)
		{
			var roomForShields = shield.Capacity - shield.Energy;
			var energyTransferredToShields = Math.Min(roomForShields, reactor.Energy);
			shield.Energy += energyTransferredToShields;
			reactor.Energy -= energyTransferredToShields;
		}
	}
}
