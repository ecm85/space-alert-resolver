using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class SkirmishersB : Skirmishers
	{
		public SkirmishersB()
			: base(StationLocation.UpperBlue)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			MoveRed();
		}

		public static string GetDisplayName()
		{
			return "Skirmishers I1-02";
		}
	}
}
