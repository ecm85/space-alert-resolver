using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public class UnstableWarheads : MinorWhiteInternalThreat
	{
		public UnstableWarheads(int timeAppears, ISittingDuck sittingDuck)
			: base(sittingDuck.RocketsComponent.Rockets.Count, 3, timeAppears, StationLocation.LowerBlue, PlayerAction.C, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
		}

		public override void PerformYAction()
		{
		}

		public override void PerformZAction()
		{
			Damage(RemainingHealth * 3);
		}

		public static string GetDisplayName()
		{
			return "Unstable Warheads";
		}
	}
}
