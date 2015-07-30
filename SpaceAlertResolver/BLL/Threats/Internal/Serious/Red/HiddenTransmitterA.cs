using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitterA : HiddenTransmitter
	{
		public HiddenTransmitterA() : base(StationLocation.LowerRed)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone);
			Damage(4);
		}
	}
}
