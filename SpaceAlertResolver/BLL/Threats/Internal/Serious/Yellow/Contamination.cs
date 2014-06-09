using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public override void PerformXAction(int currentTurn)
		{
			SittingDuck.ShiftPlayers(CurrentStations, currentTurn + 1);
		}

		public override void PerformYAction(int currentTurn)
		{
			Damage(1, CurrentZones);
		}

		public override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
			//TODO: This effect persists
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
			CurrentStations.Remove(stationLocation);
		}
	}
}
