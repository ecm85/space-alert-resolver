using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitterA : HiddenTransmitter
	{
		public HiddenTransmitterA(ExternalThreat threatToCallIn) : base(StationLocation.LowerRed, threatToCallIn)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone);
			Damage(4);
		}
	}
}
