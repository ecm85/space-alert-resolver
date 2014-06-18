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

		public Ninja()
			: base(3, 2, StationLocation.LowerBlue, PlayerAction.BattleBots)
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
			performingPlayer.IsPoisoned = true;
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
			if(!IsDefeated)
				SittingDuck.DrainReactor(CurrentZone, 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPoisonedPlayers(EnumFactory.All<StationLocation>());

			if (!IsDefeated)
			{
				var removedRocketCount = SittingDuck.GetRocketCount();
				SittingDuck.RemoveAllRockets();
				for (var i = 0; i < removedRocketCount; i++)
					SittingDuck.TakeAttack(new ThreatDamage(2, ThreatDamageType.Standard, new[] {ZoneLocation.Red}));
			}
		}

		protected override void OnHealthReducedToZero()
		{
			var anyPlayersPoisoned = SittingDuck.GetPoisonedPlayerCount(EnumFactory.All<StationLocation>()) == 0;
			if (anyPlayersPoisoned)
			{
				IsDefeated = true;
				CurrentStations.Clear();
				ThreatController.EndOfTurn -= PerformEndOfTurn;
			}
			else
				base.OnHealthReducedToZero();
			SittingDuck.UnsubscribeFromMoveIn(droneLocations, PoisonPlayer);
			SittingDuck.UnsubscribeFromMoveOut(droneLocations, PoisonPlayer);
		}

		protected override void OnReachingEndOfTrack()
		{
			if (IsDefeated)
			{
				Position = null;
				ThreatController.ThreatsMove -= PerformMove;
			}
			else
				base.OnReachingEndOfTrack();
			SittingDuck.UnsubscribeFromMoveIn(droneLocations, PoisonPlayer);
			SittingDuck.UnsubscribeFromMoveOut(droneLocations, PoisonPlayer);
		}

		public static string GetDisplayName()
		{
			return "Ninja";
		}
	}
}
