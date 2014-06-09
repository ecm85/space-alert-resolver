using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class SaboteurB : Saboteur
	{
		public override void PerformXAction()
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Saboteur I1-03";
		}
	}
}
