using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class Shambler : SeriousWhiteInternalThreat
	{
		public Shambler(int timeAppears, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, StationLocation.LowerWhite, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			if (IsAnyPlayerPresent())
				MoveBlue();
		}

		public override void PerformYAction()
		{
			if (IsAnyPlayerPresent())
				Damage(2);
			else
				Repair(1);
		}

		public override void PerformZAction()
		{
			Damage(4);
		}

		public static string GetDisplayName()
		{
			return "Shambler";
		}

		private bool IsAnyPlayerPresent()
		{
			return sittingDuck.StationByLocation[CurrentStation].Players.Any();
		}
	}
}
