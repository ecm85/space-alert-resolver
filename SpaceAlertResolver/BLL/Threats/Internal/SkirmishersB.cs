using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SkirmishersB : Skirmishers
	{
		public SkirmishersB(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck.BlueZone.UpperStation, sittingDuck)
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
