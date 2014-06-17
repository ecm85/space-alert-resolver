using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

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

		protected override void PerformXAction(int currentTurn)
		{
			MoveToMostPlayers();
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveToMostPlayers();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(5);
			SittingDuck.KnockOutPlayers(CurrentStations);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (IsDefeated)
				performingPlayer.IsKnockedOut = true;
		}

		public static string GetDisplayName()
		{
			return "Seeker";
		}

		internal void MoveToMostPlayers()
		{
			var adjacentStationGroup = new[]
			{
				CurrentStation.RedwardStationLocation(),
				CurrentStation.BluewardStationLocation(),
				CurrentStation.OppositeStationLocation()
			}
			.Where(station => station != null)
			.Select(station => new {Station = station.Value, PlayerCount = SittingDuck.GetPlayerCount(station.Value)})
			.GroupBy(station => station.PlayerCount)
			.OrderByDescending(group => group.Key)
			.FirstOrDefault();

			if (adjacentStationGroup != null && adjacentStationGroup.Count() == 1)
				MoveToNewStation(adjacentStationGroup.Single().Station);
		}
	}
}
