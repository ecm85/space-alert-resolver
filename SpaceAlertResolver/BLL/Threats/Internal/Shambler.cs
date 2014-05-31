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
				sittingDuck.TakeDamage(2, CurrentStation.ZoneLocation);
			else
				Repair(1);
		}

		public override void PerformZAction()
		{
			sittingDuck.TakeDamage(4, CurrentStation.ZoneLocation);
		}

		private bool IsAnyPlayerPresent()
		{
			//TODO: Return if any player present in current station
			return false;
		}
	}
}
