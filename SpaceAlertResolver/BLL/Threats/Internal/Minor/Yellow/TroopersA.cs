using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class TroopersA : Troopers
	{
		public TroopersA(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.LowerBlue, sittingDuck)
		{
		}

		public override void PerformYAction()
		{
			MoveRed();
		}

		public static string GetDisplayName()
		{
			return "Troopers I2-04";
		}
	}
}
