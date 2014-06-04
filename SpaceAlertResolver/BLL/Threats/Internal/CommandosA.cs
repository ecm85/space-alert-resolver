using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class CommandosA : Commandos
	{
		public CommandosA(int timeAppears, ISittingDuck sittingDuck)
			: base(timeAppears, StationLocation.LowerRed, sittingDuck)
		{
		}

		public override void PerformYAction()
		{
			if (IsDamaged)
				MoveBlue();
			else
				Damage(2);
		}

		public static string GetDisplayName()
		{
			return "Commandos SI1-01";
		}
	}
}
