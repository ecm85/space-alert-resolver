using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitterB : HiddenTransmitter
	{
		public HiddenTransmitterB() : base(StationLocation.UpperBlue)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainShield(CurrentZone);
			Damage(4);
		}

		public static string GetDisplayName()
		{
			return "Hidden Transmitter SI3-102";
		}
	}
}
