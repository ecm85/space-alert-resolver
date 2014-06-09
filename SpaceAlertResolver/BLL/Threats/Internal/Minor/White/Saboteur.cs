using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class Saboteur : MinorWhiteInternalThreat
	{
		protected Saboteur()
			: base(1, 4, StationLocation.LowerWhite, PlayerAction.BattleBots)
		{
		}

		public override void PerformYAction()
		{
			if (SittingDuck.DrainReactors(CurrentZones, 1) == 0)
				Damage(1);
		}

		public override void PerformZAction()
		{
			Damage(2);
		}
	}
}
