using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class SaboteurA : Saboteur
	{
		public override void PerformXAction(int currentTurn)
		{
			MoveRed();
		}

		public static string GetDisplayName()
		{
			return "Saboteur I1-04";
		}
	}
}
