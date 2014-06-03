using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class Shambler : SeriousWhiteInternalThreat
	{
		public Shambler(int timeAppears, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, sittingDuck.WhiteZone.LowerStation, PlayerAction.BattleBots, sittingDuck)
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
			return CurrentStation.Players.Any();
		}
	}
}
