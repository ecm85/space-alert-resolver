using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SkirmishersA : Skirmishers
	{
		public SkirmishersA(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.UpperRed, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Skirmishers I1-01";
		}
	}
}
