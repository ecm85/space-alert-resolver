using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			EnergyLeaksOut(CurrentZone);
		}

		public override void PerformYAction()
		{
			EnergyLeaksOut(CurrentZone);
		}

		public override void PerformZAction()
		{
			foreach (var zoneLocation in EnumFactory.All<ZoneLocation>())
				EnergyLeaksOut(zoneLocation);
		}

		public static string GetDisplayName()
		{
			return "Crossed Wires";
		}

		private void EnergyLeaksOut(ZoneLocation zoneLocation)
		{
			var energyDrained = sittingDuck.DrainShields(new[] { zoneLocation });
			Damage(energyDrained);
		}
	}
}
