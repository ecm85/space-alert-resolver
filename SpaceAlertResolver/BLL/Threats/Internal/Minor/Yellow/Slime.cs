using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Slime : MinorYellowInternalThreat
	{
		private readonly IList<Slime> currentProgeny;

		protected Slime(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerActionType.BattleBots)
		{
			currentProgeny = new List<Slime>();
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			SittingDuck.SubscribeToMoveIn(CurrentStations, DelayPlayer);
		}

		private static void DelayPlayer(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn + 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}

		public override bool IsDefeated
		{
			get { return base.IsDefeated && currentProgeny.All(progeny => progeny.IsDefeated); }
		}

		protected override void OnHealthReducedToZero()
		{
			SittingDuck.UnsubscribeFromMoveIn(CurrentStations, DelayPlayer);
			base.OnHealthReducedToZero();
		}

		protected abstract Slime CreateProgeny(StationLocation stationLocation);

		protected void Spread(StationLocation? stationLocation)
		{
			if (stationLocation != null && !ThreatController.DamageableInternalThreats.Any(threat => threat is Slime && threat.CurrentStation == stationLocation))
			{
				var newProgeny = CreateProgeny(stationLocation.Value);
				currentProgeny.Add(newProgeny);
				ThreatController.AddInternalThreat(newProgeny, TimeAppears, Position.GetValueOrDefault());
			}
		}
	}
}
