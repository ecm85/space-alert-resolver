using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class SkirmishersB : Skirmishers
	{
		public SkirmishersB(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.UpperBlue, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			MoveRed();
		}

		public static string GetDisplayName()
		{
			return "Skirmishers I1-02";
		}
	}
}
