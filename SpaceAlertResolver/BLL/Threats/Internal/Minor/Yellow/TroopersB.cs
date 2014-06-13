using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class TroopersB : Troopers
	{
		public TroopersB()
			: base(StationLocation.UpperRed)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Troopers I2-03";
		}
	}
}
