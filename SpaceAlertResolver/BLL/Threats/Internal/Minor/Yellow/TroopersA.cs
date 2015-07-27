using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class TroopersA : Troopers
	{
		public TroopersA()
			: base(StationLocation.LowerBlue)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveRed();
		}

		public static string GetDisplayName()
		{
			return "Troopers I2-04";
		}

		public static string GetId()
		{
			return "I2-04";
		}
	}
}
