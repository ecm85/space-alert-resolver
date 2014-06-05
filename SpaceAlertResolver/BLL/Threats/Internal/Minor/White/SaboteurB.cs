using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class SaboteurB : Saboteur
	{
		public SaboteurB(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Saboteur I1-03";
		}
	}
}
