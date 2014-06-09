using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Virus : MinorYellowInternalThreat
	{
		public Virus()
			: base(3, 3, StationLocation.UpperWhite, PlayerAction.C)
		{
		}

		public static string GetDisplayName()
		{
			return "Virus";
		}

		public override void PerformXAction()
		{
			SittingDuck.DrainAllReactors(1);
		}

		public override void PerformYAction()
		{
			//TODO: Shift all players
		}

		public override void PerformZAction()
		{
			DamageAllZones(1);
		}
	}
}
