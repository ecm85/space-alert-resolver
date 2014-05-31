using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class Alien : SeriousWhiteInternalThreat
	{
		public bool grownUp;

		public Alien(int health, int speed, int timeAppears, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, sittingDuck.WhiteZone.LowerStation, PlayerAction.BattleBots)
		{
		}

		public override void PeformXAction(SittingDuck sittingDuck)
		{
			grownUp = true;
		}

		public override void PerformYAction(SittingDuck sittingDuck)
		{
			ChangeDecks();
			//TODO: Deal (#players in new station) damage
		}

		public override void PerformZAction(SittingDuck sittingDuck)
		{
			throw new NotImplementedException("Losing is not yet implemented");
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
