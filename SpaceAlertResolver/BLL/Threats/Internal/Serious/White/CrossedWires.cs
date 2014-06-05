using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class CrossedWires : SeriousWhiteInternalThreat
	{
		public CrossedWires(int timeAppears, ISittingDuck sittingDuck)
			: base(4, 3, timeAppears, StationLocation.UpperWhite, PlayerAction.B, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.TransferEnergyToShields(new [] {CurrentZone});
			sittingDuck.EnergyLeaksOut(new[] {CurrentZone});
			var reactor = sittingDuck.ZonesByLocation[CurrentZone].LowerStation.EnergyContainer;
			EnergyLeaksOut(reactor);
		}

		public override void PerformYAction()
		{
			var reactor = sittingDuck.ZonesByLocation[CurrentZone].LowerStation.EnergyContainer;
			EnergyLeaksOut(reactor);
		}

		public override void PerformZAction()
		{
			var allReactors = sittingDuck.Zones.Select(zone => zone.LowerStation.EnergyContainer);
			foreach (var reactor in allReactors)
				EnergyLeaksOut(reactor);
		}

		public static string GetDisplayName()
		{
			return "Crossed Wires";
		}

		private void EnergyLeaksOut(EnergyContainer reactor)
		{
			Damage(reactor.Energy);
			reactor.Energy = 0;
		}
	}
}
