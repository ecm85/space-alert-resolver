using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Contamination : SeriousYellowInternalThreat
	{
		public Contamination(int timeAppears, ISittingDuck sittingDuck)
			: base(
				3,
				2,
				timeAppears,
				new List<StationLocation>
				{
					StationLocation.UpperBlue,
					StationLocation.UpperRed,
					StationLocation.LowerBlue,
					StationLocation.LowerRed
				},
				PlayerAction.BattleBots,
				sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Contamination";
		}

		public override void PeformXAction()
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
			sittingDuck.KnockOutPlayers(CurrentStations);
			//TODO: This effect persists
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			CurrentStations.Remove(stationLocation);
		}
	}
}
