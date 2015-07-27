using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Kamikaze : MinorYellowExternalThreat
	{
		public Kamikaze()
			: base(2, 5, 4)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Speed++;
			shields = 1;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Speed++;
			shields = 0;
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(6);
		}

		public static string GetDisplayName()
		{
			return "Kamikaze";
		}

		public static string GetId()
		{
			return "E2-01";
		}
	}
}
