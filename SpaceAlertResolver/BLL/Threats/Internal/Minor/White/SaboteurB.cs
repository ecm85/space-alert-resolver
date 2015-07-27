using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class SaboteurB : Saboteur
	{
		protected override void PerformXAction(int currentTurn)
		{
			MoveBlue();
		}

		public static string GetDisplayName()
		{
			return "Saboteur I1-03";
		}

		public static string GetId()
		{
			return "I1-03";
		}
	}
}
