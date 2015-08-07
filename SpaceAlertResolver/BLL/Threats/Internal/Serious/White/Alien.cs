using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Common;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class Alien : SeriousWhiteInternalThreat
	{
		private bool grownUp;

		public Alien()
			: base(2, 2, StationLocation.LowerWhite, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			grownUp = true;
		}

		protected override void PerformYAction(int currentTurn)
		{
			ChangeDecks();
			Damage(SittingDuck.GetPlayerCount(CurrentStation));
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}
		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (grownUp && !isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
