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

		public override void PerformXAction()
		{
			throw new NotImplementedException();
			//TODO: Delay players in each of current stations
		}

		public override void PerformYAction()
		{
			Damage(1, CurrentZones);
		}

		public override void PerformZAction()
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
