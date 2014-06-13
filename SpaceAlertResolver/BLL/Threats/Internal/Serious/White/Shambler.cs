using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class Shambler : SeriousWhiteInternalThreat
	{
		public Shambler()
			: base(2, 2, StationLocation.LowerWhite, PlayerAction.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (IsAnyPlayerPresent())
				MoveBlue();
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (IsAnyPlayerPresent())
				Damage(2);
			else
				Repair(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(4);
		}

		public static string GetDisplayName()
		{
			return "Shambler";
		}

		private bool IsAnyPlayerPresent()
		{
			return SittingDuck.GetPlayerCount(CurrentStation) != 0;
		}
	}
}
