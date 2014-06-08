using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class Saboteur : MinorWhiteInternalThreat
	{
		protected Saboteur(int timeAppears, ISittingDuck sittingDuck)
			: base(1, 4, timeAppears, StationLocation.LowerWhite, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PerformYAction()
		{
			if (sittingDuck.DrainReactors(CurrentZones, 1) == 0)
				Damage(1);
		}

		public override void PerformZAction()
		{
			Damage(2);
		}
	}
}
