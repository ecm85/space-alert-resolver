using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class HackedShieldsA : HackedShields
	{
		public HackedShieldsA(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck.RedZone.UpperStation, sittingDuck)
		{
		}

		public override string GetDisplayName()
		{
			return "Hacked Shields I1-06";
		}
	}
}
