using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Red
{
	public class Parasite : SeriousRedInternalThreat
	{
		private Player attachedPlayer;

		public Parasite()
			: base(1, 2, new List<StationLocation>(), PlayerActionType.BattleBots)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			SittingDuck.SubscribeToMoveIn(EnumFactory.All<StationLocation>().Where(station => station.IsOnShip()), AttachToPlayer);
		}

		private void AttachToPlayer(Player performingPlayer, int currentTurn)
		{
			attachedPlayer = performingPlayer;
			SittingDuck.UnsubscribeFromMoveIn(EnumFactory.All<StationLocation>().Where(station => station.IsOnShip()), AttachToPlayer);
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (attachedPlayer != null && !attachedPlayer.IsKnockedOut && attachedPlayer.Interceptors != null)
				SittingDuck.DrainEnergyContainer(attachedPlayer.CurrentStation.StationLocation, 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			var otherPlayersInStation = SittingDuck.GetPlayersInStation(attachedPlayer.CurrentStation.StationLocation)
				.Except(new [] {attachedPlayer});
			foreach (var player in otherPlayersInStation)
				player.IsKnockedOut = true;
		}

		protected override void PerformZAction(int currentTurn)
		{
			if (attachedPlayer != null)
			{
				if (attachedPlayer.Interceptors != null)
					SittingDuck.TakeAttack(new ThreatDamage(5, ThreatDamageType.Standard, new[] {ZoneLocation.White}));
				else
					Damage(5);
			}
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if(IsDefeated)
				attachedPlayer.IsKnockedOut = true;
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}

		public override bool CanBeTargetedBy(StationLocation stationLocation, PlayerActionType playerActionType, Player performingPlayer)
		{
			return ActionType == playerActionType && CurrentStations.Contains(performingPlayer.CurrentStation.StationLocation) && performingPlayer != attachedPlayer;
		}

		protected override void OnThreatTerminated()
		{
			base.OnThreatTerminated();
			SittingDuck.UnsubscribeFromMoveIn(EnumFactory.All<StationLocation>().Where(station => station.IsOnShip()), AttachToPlayer);
		}

		public static string GetDisplayName()
		{
			return "Cyber Gremlin";
		}

		public static string GetId()
		{
			return "SI3-107";
		}
	}
}
