using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Virus : MinorYellowInternalThreat
	{
		public Virus(int timeAppears, ISittingDuck sittingDuck)
			: base(3, 3, timeAppears, StationLocation.UpperWhite, PlayerAction.C, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Virus";
		}

		public override void PeformXAction()
		{
			foreach(var zone in sittingDuck.Zones)
				zone.DrainReactors(1);
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
