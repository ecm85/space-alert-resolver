using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class CommandosB : Commandos
	{
		public CommandosB()
			: base(StationLocation.UpperBlue)
		{
		}

		public override void PerformYAction()
		{
			if (IsDamaged)
				MoveRed();
			else
				Damage(2);
		}

		public static string GetDisplayName()
		{
			return "Commandos SI1-02";
		}
	}
}
