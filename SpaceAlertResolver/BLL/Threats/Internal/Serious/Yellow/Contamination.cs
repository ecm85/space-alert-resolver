using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Contamination : SeriousYellowInternalThreat
	{
		public Contamination()
			: base(
				3,
				2,
				new List<StationLocation>
				{
					StationLocation.UpperBlue,
					StationLocation.UpperRed,
					StationLocation.LowerBlue,
					StationLocation.LowerRed
				},
				PlayerAction.BattleBots)
		{
		}

		public static string GetDisplayName()
		{
			return "Contamination";
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.ShiftPlayers(CurrentStations, currentTurn + 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Damage(1, CurrentZones);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
			//TODO: This effect persists
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			CurrentStations.Remove(stationLocation);
		}
	}
}
