using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class Frigate : SeriousWhiteExternalThreat
	{
		public Frigate()
			: base(2, 7, 2)
		{
		}

		public override void PerformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			Attack(3);
		}

		public override void PerformZAction()
		{
			Attack(4);
		}

		public static string GetDisplayName()
		{
			return "Frigate";
		}
	}
}
