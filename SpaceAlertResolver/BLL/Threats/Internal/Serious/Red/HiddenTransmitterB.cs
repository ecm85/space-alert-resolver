using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitterB : HiddenTransmitter
	{
		public HiddenTransmitterB(ExternalThreat threatToCallIn) : base(StationLocation.UpperBlue, threatToCallIn)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainShield(CurrentZone);
			Damage(4);
		}
	}
}
