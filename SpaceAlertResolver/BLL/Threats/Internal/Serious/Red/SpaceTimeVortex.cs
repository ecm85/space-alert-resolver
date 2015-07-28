﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class SpaceTimeVortex : SeriousRedInternalThreat
	{
		private readonly InternalThreat threatToCallIn;

		public SpaceTimeVortex(InternalThreat threatToCallIn)
			: base(3, 2, StationLocation.LowerWhite, PlayerActionType.C)
		{
			this.threatToCallIn = threatToCallIn;
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.ShiftPlayers(EnumFactory.All<StationLocation>(), currentTurn + 1, true);
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
			threatToCallIn.Initialize(SittingDuck, ThreatController);
			threatToCallIn.TimeAppears = currentTurn;
			threatToCallIn.PlaceOnBoard(Track);
			while(threatToCallIn.Position != null)
				threatToCallIn.Move(currentTurn);
		}

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
