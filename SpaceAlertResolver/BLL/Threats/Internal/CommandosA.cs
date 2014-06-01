using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class CommandosA : Commandos
	{
		public CommandosA(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck.RedZone.LowerStation, sittingDuck)
		{
		}

		public override void PerformYAction()
		{
			if (IsDamaged)
				MoveBlue();
			else
				Damage(2);
		}
	}
}
