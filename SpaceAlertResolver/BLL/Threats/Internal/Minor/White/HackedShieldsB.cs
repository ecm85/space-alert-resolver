using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class HackedShieldsB : HackedShields
	{
		public HackedShieldsB()
			: base(StationLocation.UpperBlue)
		{
		}

		public static string GetDisplayName()
		{
			return "Hacked Shields I1-05";
		}
	}
}
