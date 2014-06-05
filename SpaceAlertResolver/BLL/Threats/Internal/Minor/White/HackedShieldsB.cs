using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class HackedShieldsB : HackedShields
	{
		public HackedShieldsB(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.UpperBlue, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Hacked Shields I1-05";
		}
	}
}
