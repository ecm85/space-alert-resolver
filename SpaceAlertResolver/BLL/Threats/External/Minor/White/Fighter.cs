using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class Fighter : MinorWhiteExternalThreat
	{
		public Fighter()
			: base(2, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(3);
		}

		public static string GetDisplayName()
		{
			return "Fighter";
		}

		public static string GetId()
		{
			return "E1-07";
		}
	}
}
