using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class SkirmishersA : Skirmishers
	{
		public SkirmishersA()
			: base(StationLocation.UpperRed)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Skirmishers I1-01";
		}
	}
}
