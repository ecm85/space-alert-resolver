using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class SpaceTimeVortex : SeriousRedInternalThreat
	{
		public SpaceTimeVortex()
			: base(3, 2, StationLocation.LowerWhite, PlayerAction.C)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			//TODO: All players are shifted with previous action duplicated
		}

		protected override void PerformYAction(int currentTurn)
		{
			//TODO: Players in red move to blue and vice versa
		}

		protected override void PerformZAction(int currentTurn)
		{
			//TODO: Perform new threats abilities and discard it
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			//TODO: all players change decks
		}

		public static string GetDisplayName()
		{
			return "Space Time Vortex";
		}
	}
}
