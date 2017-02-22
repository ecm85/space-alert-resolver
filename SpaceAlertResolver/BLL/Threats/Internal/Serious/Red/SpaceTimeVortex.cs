using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class SpaceTimeVortex : SeriousRedInternalThreat, IThreatWithBonusThreat<InternalThreat>
	{
		public InternalThreat BonusThreat { get; set; }

		public SpaceTimeVortex()
			: base(3, 2, StationLocation.LowerWhite, PlayerActionType.Charlie)
		{
		}

		public override bool NeedsBonusInternalThreat { get { return true; } }

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.ShiftPlayersAndRepeatPreviousAction(EnumFactory.All<StationLocation>(), currentTurn + 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			var upperRedPlayers = SittingDuck.GetPlayersInStation(StationLocation.UpperRed);
			var lowerRedPlayers = SittingDuck.GetPlayersInStation(StationLocation.LowerRed);
			var upperBluePlayers = SittingDuck.GetPlayersInStation(StationLocation.UpperBlue);
			var lowerBluePlayers = SittingDuck.GetPlayersInStation(StationLocation.LowerBlue);
			SittingDuck.TeleportPlayers(upperRedPlayers, StationLocation.UpperBlue);
			SittingDuck.TeleportPlayers(lowerRedPlayers, StationLocation.LowerBlue);
			SittingDuck.TeleportPlayers(upperBluePlayers, StationLocation.UpperRed);
			SittingDuck.TeleportPlayers(lowerBluePlayers, StationLocation.LowerRed);
		}

		protected override void PerformZAction(int currentTurn)
		{
			ThreatController.AddInternalThreat(BonusThreat, currentTurn);
			while(BonusThreat.Position >= 0)
				BonusThreat.Move(currentTurn);
		}

		public override string Id { get; } = "SI3-104";
		public override string DisplayName { get; } = "Space Time Vortex";
		public override string FileName { get; } = "SpaceTimeVortex";

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			TeleportPlayersToOppositeDecks();
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}

		private void TeleportPlayersToOppositeDecks()
		{
			var playersByStation = EnumFactory.All<StationLocation>()
				.Where(stationLocation => stationLocation.IsOnShip())
				.Select(stationLocation => new
				{
					Players = SittingDuck.GetPlayersInStation(stationLocation).ToList(),
					StationLocation = stationLocation
				})
				.ToList();
			foreach (var playerList in playersByStation)
				SittingDuck.TeleportPlayers(
					playerList.Players,
					playerList.StationLocation.OppositeStationLocation().GetValueOrDefault());
		}
	}
}
