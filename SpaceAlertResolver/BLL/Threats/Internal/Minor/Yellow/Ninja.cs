using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			SittingDuck.SubscribeToMoveIn(droneLocations, PoisonPlayer);
			SittingDuck.SubscribeToMoveOut(droneLocations, PoisonPlayer);
		}

		private void PoisonPlayer(Player performingPlayer, int currentTurn)
		{
			if (poisonedPlayers == null)
			{
				poisonedPlayers = new PoisonedPlayers(Type, difficulty);
				ThreatController.AddInternalThreat(poisonedPlayers, TimeAppears, Position.GetValueOrDefault());
			}
			poisonedPlayers.PoisonPlayer(performingPlayer);
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
			var removedRocketCount = SittingDuck.GetRocketCount();
			SittingDuck.RemoveAllRockets();
			for (var i = 0;i < removedRocketCount;i++)
				SittingDuck.TakeAttack(new ThreatDamage(2, ThreatDamageType.Standard, new[] {ZoneLocation.Red}));
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			SittingDuck.UnsubscribeFromMoveIn(droneLocations, PoisonPlayer);
			SittingDuck.UnsubscribeFromMoveOut(droneLocations, PoisonPlayer);
		}

		public static string GetDisplayName()
		{
			return "Ninja";
		}

		public static string GetId()
		{
			return "I2-102";
		}

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

			public override int Points
			{
				get { return 0; }
			}

			public override bool IsDefeated
			{
				get { return false; }
			}

			public override bool IsSurvived
			{
				get { return false; }
			}

			public override bool IsDamageable
			{
				get { return false; }
			}

			public static string GetId()
			{
				return "I2-102-S";
			}
		}
	}
}
