using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SkirmishersB : MinorWhiteInternalThreat
	{
		protected SkirmishersB(int timeAppears, SittingDuck sittingDuck)
			: base(0, 1, 3, timeAppears, sittingDuck.BlueZone.UpperStation, PlayerAction.BattleBots)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			CurrentStation = CurrentStation.BluewardStation;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			CurrentStation = CurrentStation.OppositeDeckStation;
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(3, CurrentStation.ZoneLocation);
		}

		public override void TakeDamage(int damage)
		{
			//TODO: Destroy battlebots
			//TODO: handle actually having battlebots, only allow action if they have em, and disable battlebots when hit back
			base.TakeDamage(damage);
		}
	}
}
