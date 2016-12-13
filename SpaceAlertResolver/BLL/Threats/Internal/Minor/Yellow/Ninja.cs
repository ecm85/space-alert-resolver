using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class Ninja : MinorYellowInternalThreat
	{
		private IList<StationLocation> droneLocations = new List<StationLocation>();
		private PoisonedPlayers poisonedPlayers;

		public Ninja()
			: base(3, 2, StationLocation.LowerBlue, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			droneLocations = AdjacentLocations();
			SittingDuck.SubscribeToMovingIn(droneLocations, PoisonPlayer);
			SittingDuck.SubscribeToMovingOut(droneLocations, PoisonPlayer);
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
			SittingDuck.UnsubscribeFromMovingIn(droneLocations, PoisonPlayer);
			SittingDuck.UnsubscribeFromMovingOut(droneLocations, PoisonPlayer);
		}

		private class PoisonedPlayers : InternalThreat
		{
			private readonly HashSet<Player> poisonedPlayers;

			public PoisonedPlayers(ThreatType threatType, ThreatDifficulty threatDifficulty)
				: base(threatType, threatDifficulty, 0, 0, new List<StationLocation>())
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

			public override int Points => 0;

			public override bool IsDefeated => false;

			public override bool IsSurvived => false;

			public override bool IsDamageable => false;
		}
	}
}
