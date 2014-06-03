using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class HackedShieldsB : HackedShields
	{
		public HackedShieldsB(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck.BlueZone.UpperStation, sittingDuck)
		{
		}

		public override string GetDisplayName()
		{
			return "Hacked Shields I1-05";
		}
	}
}
