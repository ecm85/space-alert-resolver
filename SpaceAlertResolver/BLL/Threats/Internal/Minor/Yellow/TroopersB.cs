using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class TroopersB : Troopers
	{
		public TroopersB(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.UpperRed, sittingDuck)
		{
		}

		public override void PerformYAction()
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Troopers I2-03";
		}
	}
}
