using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SkirmishersA : Skirmishers
	{
		public SkirmishersA(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck.RedZone.UpperStation, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			MoveBlue();
		}

		public override string GetDisplayName()
		{
			return "Skirmishers I1-01";
		}
	}
}
