using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Ninja : MinorYellowInternalThreat
	{
		private IList<StationLocation> DroneLocations => WarningIndicatorStations;
		private PoisonedPlayers poisonedPlayers;

		public Ninja()
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
				poisonedPlayers = new PoisonedPlayers(ThreatType, Difficulty);
				ThreatController.AddInternalThreat(poisonedPlayers, TimeAppears, Position.GetValueOrDefault());
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
			var removedRocketCount = SittingDuck.RocketCount;
			SittingDuck.RemoveAllRockets();
			for (var i = 0;i < removedRocketCount;i++)
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

			public PoisonedPlayers(ThreatType threatType, ThreatDifficulty threatDifficulty)
				: base(threatType, threatDifficulty, 0, 0, new List<StationLocation>(), null)
			{
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
					player.IsKnockedOut = true;
			}

			public override string Id { get { throw new NotImplementedException(); } }
			public override string DisplayName { get { throw new NotImplementedException(); } }
			public override string FileName { get { throw new NotImplementedException(); } }

			public override int Points => 0;

			public override bool IsDefeated => false;

			public override bool IsSurvived => false;

			public override bool IsDamageable => false;
		}
	}
}
