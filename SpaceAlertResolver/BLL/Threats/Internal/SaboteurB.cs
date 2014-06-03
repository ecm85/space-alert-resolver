using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SaboteurB : Saboteur
	{
		public SaboteurB(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			MoveBlue();
		}

		public override string GetDisplayName()
		{
			return "Saboteur I1-03";
		}
	}
}
