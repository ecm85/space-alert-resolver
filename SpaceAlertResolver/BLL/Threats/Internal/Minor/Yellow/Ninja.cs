using System.Collections.Generic;
using System.Linq;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Ninja : MinorYellowInternalThreat
	{
		private IList<StationLocation> DroneLocations => WarningIndicatorStations;
		private PoisonedPlayers poisonedPlayers;

		internal Ninja()
			: base(3, 2, StationLocation.LowerBlue, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			foreach (var adjacentLocation in AdjacentLocations())
				DroneLocations.Add(adjacentLocation);
			SittingDuck.SubscribeToMovingIn(DroneLocations, PoisonPlayer);
			SittingDuck.SubscribeToMovingOut(DroneLocations, PoisonPlayer);
		}

		private void PoisonPlayer(object sender, PlayerMoveEventArgs args)
		{
			if (poisonedPlayers == null)
			{
				poisonedPlayers = new PoisonedPlayers(this);
				poisonedPlayers.Initialize(SittingDuck, ThreatController);
				ThreatController.AddInternalThreat(poisonedPlayers, TimeAppears, Position);
			}
			poisonedPlayers.PoisonPlayer(args.MovingPlayer);
		}

		private IList<StationLocation> AdjacentLocations()
		{
			return new [] {CurrentStation.BluewardStationLocation(), CurrentStation.RedwardStationLocation(), CurrentStation.OppositeStationLocation()}
				.Where(stationLocation => stationLocation != null)
				.Select(stationLocation => stationLocation.Value)
				.ToList();
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone, 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			var removedRocketCount = SittingDuck.RemoveAllRockets();
			for (var i = 0; i < removedRocketCount; i++)
				SittingDuck.TakeAttack(new ThreatDamage(2, ThreatDamageType.Standard, new[] {ZoneLocation.Red}));
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			SittingDuck.UnsubscribeFromMovingIn(DroneLocations, PoisonPlayer);
			SittingDuck.UnsubscribeFromMovingOut(DroneLocations, PoisonPlayer);
		}

		public override string Id { get; } = "I2-102";
		public override string DisplayName { get; } = "Ninja";
		public override string FileName { get; } = "Ninja";

		private class PoisonedPlayers : InternalThreat
		{
			private readonly HashSet<Player> poisonedPlayers;

			internal PoisonedPlayers(InternalThreat parent)
				: base(parent.ThreatType, parent.Difficulty, 0, parent.Speed, new List<StationLocation>(), null)
			{
				Parent = parent;
				poisonedPlayers = new HashSet<Player>();
			}

			public void PoisonPlayer(Player player)
			{
				poisonedPlayers.Add(player);
			}

			protected override void PerformXAction(int currentTurn)
			{
			}

			protected override void PerformYAction(int currentTurn)
			{
			}

			protected override void PerformZAction(int currentTurn)
			{
				foreach (var player in poisonedPlayers)
					player.KnockOut();
			}

			public override string Id { get; } = "I2-102";
			public override string DisplayName { get; } = "Ninja";
			public override string FileName { get; } = "Ninja";

			protected override bool IsDefeatedWhenHealthReachesZero => false;
			protected override bool IsSurvivedWhenReachingEndOfTrack => false;
		}
	}
}
