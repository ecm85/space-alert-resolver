using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class HackedShieldsA : HackedShields
	{
		public HackedShieldsA()
			: base(StationLocation.UpperRed)
		{
		}

		public static string GetDisplayName()
		{
			return "Hacked Shields I1-06";
		}
	}
}
