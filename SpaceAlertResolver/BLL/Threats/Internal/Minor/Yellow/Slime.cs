using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Slime : MinorYellowInternalThreat
	{
		protected IList<MinorYellowInternalThreat> CurrentProgeny { get; }

		public override IList<StationLocation> DisplayStations
		{
			get
			{
				return base.DisplayStations
					.Concat(CurrentProgeny.SelectMany(progeny => progeny.CurrentStations))
					.ToList();
			}
		}

		protected Slime(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerActionType.BattleBots)
		{
			CurrentProgeny = new List<MinorYellowInternalThreat>();
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			SittingDuck.SubscribeToMovingIn(CurrentStations, DelayPlayer);
		}

		private static void DelayPlayer(object sender, PlayerMoveEventArgs args)
		{
			args.MovingPlayer.Shift(args.CurrentTurn + 1);
		}

		protected override void OnHealthReducedToZero()
		{
			CurrentStations.Remove(CurrentStation);
			SittingDuck.UnsubscribeFromMovingIn(CurrentStations, DelayPlayer);
			base.OnHealthReducedToZero();
		}

		protected override void OnThreatTerminated()
		{
			CheckForTerminated();
		}

		protected void CheckForTerminated()
		{
			if (IsDefeated || IsSurvived)
				base.OnThreatTerminated();
		}

		protected abstract MinorYellowInternalThreat CreateProgeny(StationLocation stationLocation);

		public void Spread(StationLocation? stationLocation)
		{
			if (stationLocation != null && !ThreatController.DamageableInternalThreats.Any(threat => threat is ProgenySlime && threat.CurrentStation == stationLocation))
			{
				var newProgeny = CreateProgeny(stationLocation.Value);
				CurrentProgeny.Add(newProgeny);
				newProgeny.Initialize(SittingDuck, ThreatController);
				ThreatController.AddInternalThreat(newProgeny, TimeAppears, Position.GetValueOrDefault());
			}
		}

		protected abstract class ProgenySlime : MinorYellowInternalThreat
		{
			protected ProgenySlime(int health, int speed, StationLocation currentStation, PlayerActionType actionType) : base(health, speed, currentStation, actionType)
			{
			}

			public override bool ShowOnTrack { get { return false; } }

			public override int Points
			{
				get { return 0; }
			}

			public override bool IsSurvived
			{
				get { return false; }
			}

			protected override void PerformXAction(int currentTurn)
			{
			}

			protected override void PerformZAction(int currentTurn)
			{
				Damage(2);
			}

			protected override void OnThreatTerminated()
			{
				base.OnThreatTerminated();
				ParentSlime.CheckForTerminated();
			}

			protected Slime ParentSlime { get; set; }
		}
	}
}
