using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class CrossedWires : SeriousWhiteInternalThreat
	{
		public CrossedWires()
			: base(4, 3, StationLocation.UpperWhite, PlayerAction.B)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.TransferEnergyToShields(new [] {CurrentZone});
			EnergyLeaksOut(CurrentZone);
		}

		protected override void PerformYAction(int currentTurn)
		{
			EnergyLeaksOut(CurrentZone);
		}

		protected override void PerformZAction(int currentTurn)
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
			var energyDrained = SittingDuck.DrainShields(new[] { zoneLocation });
			Damage(energyDrained);
		}
	}
}
