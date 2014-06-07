using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Seeker : SeriousYellowInternalThreat
	{
		public Seeker(int timeAppears, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, StationLocation.UpperWhite, PlayerAction.BattleBots, sittingDuck)
		{
		}

		protected override int GetPointsForDefeating()
		{
			return ThreatPoints.GetPointsForDefeatingSeeker();
		}

		public override void PeformXAction()
		{
			MoveToMostPlayers();
		}

		public override void PerformYAction()
		{
			MoveToMostPlayers();
		}

		public override void PerformZAction()
		{
			Damage(5);
			sittingDuck.KnockOutPlayers(CurrentStations);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (IsDestroyed)
				performingPlayer.IsKnockedOut = true;
		}

		public static string GetDisplayName()
		{
			return "Seeker";
		}

		private void MoveToMostPlayers()
		{
			//TODO: Move to most players
		}
	}
}
