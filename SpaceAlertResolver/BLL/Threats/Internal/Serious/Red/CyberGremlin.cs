using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Red
{
	public class CyberGremlin : SeriousRedInternalThreat
	{
		private IList<Sabotage> AllSabotage { get; } = new List<Sabotage>();

		public override IList<StationLocation> DisplayOnTrackStations
		{
			get
			{
				return base.DisplayOnTrackStations
					.Concat(AllSabotage
						.Where(sabotage => sabotage.IsOnShip)
						.Select(sabotage => sabotage.CurrentStation))
					.Distinct()
					.ToList();
			}
		}

		public CyberGremlin()
			: base(1, 2, StationLocation.UpperRed, PlayerActionType.BattleBots, 1)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			ThreatController.JumpingToHyperspace += OnJumpingToHyperspace;
		}

		private void OnJumpingToHyperspace(object sender, EventArgs args)
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
				new Sabotage(ThreatType, Difficulty, CurrentStation, PlayerActionType.Alpha, this),
				new Sabotage(ThreatType, Difficulty, CurrentStation, PlayerActionType.Bravo, this),
				new Sabotage(ThreatType, Difficulty, CurrentStation, PlayerActionType.Charlie, this)
			};
			foreach (var newThreat in newThreats)
			{
				AllSabotage.Add(newThreat);
				newThreat.Initialize(SittingDuck, ThreatController);
				newThreat.SetThreatStatus(ThreatStatus.NotAppeared, false);
				newThreat.SetThreatStatus(ThreatStatus.OnShip, true);
				ThreatController.AddInternalTracklessThreat(newThreat);
			}
		}

		protected override void OnHealthReducedToZero()
		{
			OnThreatTerminated();
			ThreatController.JumpingToHyperspace -= OnJumpingToHyperspace;
		}

		public override string Id { get; } = "SI3-106";
		public override string DisplayName { get; } = "Cyber Gremlin";
		public override string FileName { get; } = "CyberGremlin";

		private class Sabotage : InternalThreat
		{
			public Sabotage(ThreatType threatType, ThreatDifficulty threatDifficulty, StationLocation currentStation, PlayerActionType actionType, CyberGremlin cyberGremlin)
				: base(threatType, threatDifficulty, 1, 0, currentStation, actionType)
			{
				Parent = cyberGremlin;
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

			public override string Id { get; } = "SI3-106-X";
			public override string DisplayName => null;
			public override string FileName
			{
				get
				{
					switch (ActionType)
					{
						case PlayerActionType.Alpha:
							return "Alpha";
						case PlayerActionType.Bravo:
							return "Bravo";
						case PlayerActionType.Charlie:
							return "Charlie";
						default:
							throw new InvalidOperationException();
					}
				}
			}

			protected override bool IsDefeatedWhenHealthReachesZero => false;
			protected override bool IsSurvivedWhenReachingEndOfTrack => false;

			public override bool IsDamageable { get; } = true;
		}
	}
}
