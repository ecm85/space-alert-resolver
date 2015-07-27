using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class ManOfWar : SeriousWhiteExternalThreat
	{
		public ManOfWar()
			: base(2, 9, 1)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(2);
			Speed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(3);
			shields++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(3);
		}

		public static string GetDisplayName()
		{
			return "Man-Of-War";
		}

		public static string GetId()
		{
			return "SE1-02";
		}
	}
}
