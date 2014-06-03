using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SaboteurA : Saboteur
	{
		public SaboteurA(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			MoveRed();
		}

		public override string GetDisplayName()
		{
			return "Saboteur I1-04";
		}
	}
}
