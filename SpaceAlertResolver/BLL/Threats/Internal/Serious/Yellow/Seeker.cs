using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Seeker : SeriousYellowInternalThreat
	{
		public Seeker()
			: base(2, 2, StationLocation.UpperWhite, PlayerAction.BattleBots)
		{
		}

		protected override int GetPointsForDefeating()
		{
			return ThreatPoints.GetPointsForDefeatingSeeker();
		}

		public override void PerformXAction()
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
			SittingDuck.KnockOutPlayers(CurrentStations);
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
			if (isDefeated)
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
