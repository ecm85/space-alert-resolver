using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class RabidBeast : SeriousRedInternalThreat
	{
		public RabidBeast()
			: base(2, 2, StationLocation.UpperBlue, PlayerAction.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.InfectPlayers(CurrentStation);
			MoveRed();
			MoveRed();
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (!IsDefeated)

			SittingDuck.InfectPlayers(CurrentStation);
			ChangeDecks();
			MoveBlue();
		}

		protected override void PerformZAction(int currentTurn)
		{
			if (!IsDefeated)
				Damage(4);
			//TODO: Each infected, non-knocked out player deals 2 damage (even if threat is destroyed)
			throw new NotImplementedException();
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
