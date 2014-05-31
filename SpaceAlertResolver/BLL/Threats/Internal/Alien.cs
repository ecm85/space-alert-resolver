using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class Alien : SeriousWhiteInternalThreat
	{
		public bool grownUp;

		public Alien(int timeAppears, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, sittingDuck.WhiteZone.LowerStation, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			grownUp = true;
		}

		public override void PerformYAction()
		{
			ChangeDecks();
			sittingDuck.TakeDamage(CurrentStation.Players.Count, CurrentStation.ZoneLocation);
		}

		public override void PerformZAction()
		{
			//TODO: Lose
		}

		public override InternalPlayerDamageResult TakeDamage(int damage)
		{
			var result = base.TakeDamage(damage);
			if (grownUp)
				result.BattleBotsDisabled = true;
			return result;
		}
	}
}
