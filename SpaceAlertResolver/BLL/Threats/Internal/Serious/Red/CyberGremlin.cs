﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Red
{
	public class CyberGremlin : SeriousRedInternalThreat
	{
		public CyberGremlin()
			: base(1, 2, StationLocation.UpperRed, PlayerActionType.BattleBots, 1)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			ThreatController.JumpingToHyperspace += OnJumpingToHyperspace;
		}

		private void OnJumpingToHyperspace()
		{
			SittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>());
		}

		protected override void PerformXAction(int currentTurn)
		{
			var energyDrained = SittingDuck.DrainReactor(CurrentZone);
			Speed += energyDrained;
		}

		protected override void PerformYAction(int currentTurn)
		{
			SabotageAllSystems();
			MoveBlue();
		}

		protected override void PerformZAction(int currentTurn)
		{
			SabotageAllSystems();
		}

		private void SabotageAllSystems()
		{
			var newThreats = new[]
			{
				new Sabotage(Type, Difficulty, CurrentStation, PlayerActionType.A),
				new Sabotage(Type, Difficulty, CurrentStation, PlayerActionType.B),
				new Sabotage(Type, Difficulty, CurrentStation, PlayerActionType.C)
			};
			foreach (var newThreat in newThreats)
			{
				newThreat.Initialize(SittingDuck, ThreatController);
				ThreatController.AddInternalThreat(newThreat, TimeAppears);
			}
		}

		protected override void OnHealthReducedToZero()
		{
			OnThreatTerminated();
			ThreatController.JumpingToHyperspace -= OnJumpingToHyperspace;
		}

		private class Sabotage : InternalThreat
		{
			public Sabotage(ThreatType threatType, ThreatDifficulty threatDifficulty, StationLocation currentStation, PlayerActionType actionType)
				: base(threatType, threatDifficulty, 1, 0, currentStation, actionType)
			{
			}

			protected override void PerformXAction(int currentTurn)
			{
			}

			protected override void PerformYAction(int currentTurn)
			{
			}

			protected override void PerformZAction(int currentTurn)
			{
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

			public override void PlaceOnBoard(Track track, int? trackPosition)
			{
				base.PlaceOnBoard(null, null);
			}
		}
	}
}
