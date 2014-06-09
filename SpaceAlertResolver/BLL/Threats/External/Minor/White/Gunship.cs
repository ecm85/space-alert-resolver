using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class Gunship : MinorWhiteExternalThreat
	{
		public Gunship()
			: base(2, 5, 2)
		{
		}

		public override void PerformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			Attack(2);
		}

		public override void PerformZAction()
		{
			Attack(3);
		}

		public static string GetDisplayName()
		{
			return "Gunship";
		}
	}
}
