using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class SkirmishersB : MinorWhiteInternalThreat
	{
		protected SkirmishersB(int timeAppears, SittingDuck sittingDuck)
			: base(1, 3, timeAppears, sittingDuck.BlueZone.UpperStation, PlayerAction.BattleBots)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			MoveBlue();
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			ChangeDecks();
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			sittingDuck.TakeDamage(3, CurrentStation.ZoneLocation);
		}

		public override InternalPlayerDamageResult TakeDamage(int damage)
		{
			var result = base.TakeDamage(damage);
			result.BattleBotsDisabled = true;
			return result;
		}
	}
}
