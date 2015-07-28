using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class CrossedWires : SeriousWhiteInternalThreat
	{
		public CrossedWires()
			: base(4, 3, StationLocation.UpperWhite, PlayerActionType.B)
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
		private void EnergyLeaksOut(ZoneLocation zoneLocation)
		{
			var energyDrained = SittingDuck.DrainShield(zoneLocation);
			Damage(energyDrained);
		}
	}
}
