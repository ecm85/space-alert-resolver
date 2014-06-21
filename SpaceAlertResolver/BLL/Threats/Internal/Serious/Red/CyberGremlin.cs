using System;
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
			: base(1, 2, StationLocation.UpperRed, PlayerAction.BattleBots, 1)
		{
		}

		protected override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
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
				new Sabotage(type, difficulty, CurrentStation, PlayerAction.A),
				new Sabotage(type, difficulty, CurrentStation, PlayerAction.B),
				new Sabotage(type, difficulty, CurrentStation, PlayerAction.C)
			};
			foreach (var newThreat in newThreats)
			{
				newThreat.Initialize(SittingDuck, ThreatController, TimeAppears);
				newThreat.PlaceOnTrack();
				ThreatController.AddInternalThreat(newThreat);
			}
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnThreatTerminated();
			ThreatController.JumpingToHyperspace -= OnJumpingToHyperspace;
		}

		public static string GetDisplayName()
		{
			return "Cyber Gremlin";
		}

		private class Sabotage : InternalThreat
		{
			public Sabotage(ThreatType threatType, ThreatDifficulty threatDifficulty, StationLocation currentStation, PlayerAction actionType)
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

			public void PlaceOnTrack()
			{
				Position = -1;
				HasBeenPlaced = true;
			}
		}
	}
}
