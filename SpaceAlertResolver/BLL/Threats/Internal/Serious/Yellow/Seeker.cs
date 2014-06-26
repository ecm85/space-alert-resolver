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
			: base(2, 2, StationLocation.UpperWhite, PlayerActionType.BattleBots)
		{
		}

		public override int GetPointsForDefeating()
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
			var adjacentStations =
				new[]
				{
					new {NewStation = CurrentStation.RedwardStationLocation(), MoveCommand = new Action(MoveRed)},
					new {NewStation = CurrentStation.BluewardStationLocation(), MoveCommand = new Action(MoveBlue)},
					new {NewStation = CurrentStation.OppositeStationLocation(), MoveCommand = new Action(ChangeDecks)}
				}
				.Where(station => station.NewStation != null);

			var adjacentStationGroupWithMostPlayers = adjacentStations
				.Select(station => new {Station = station, PlayerCount = SittingDuck.GetPlayerCount(station.NewStation.Value)})
				.GroupBy(station => station.PlayerCount)
				.OrderByDescending(group => group.Key)
				.First();

			if (adjacentStationGroupWithMostPlayers.Count() == 1)
				adjacentStationGroupWithMostPlayers.Single().Station.MoveCommand();
		}
	}
}
