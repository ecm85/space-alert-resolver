using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class HackedShieldsA : HackedShields
	{
		public HackedShieldsA(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.UpperRed, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Hacked Shields I1-06";
		}
	}
}
