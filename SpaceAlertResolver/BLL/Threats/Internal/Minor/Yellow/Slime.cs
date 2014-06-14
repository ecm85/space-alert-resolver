using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Slime : MinorYellowInternalThreat
	{
		private readonly IList<Slime> currentProgeny;

		protected Slime(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerAction.BattleBots)
		{
			currentProgeny = new List<Slime>();
		}

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears)
		{
			base.Initialize(sittingDuck, threatController, timeAppears);
			sittingDuck.SubscribeToMoveIn(CurrentStations, DelayPlayer);
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
			if (stationLocation != null && !SittingDuck.GetThreatsInStation(stationLocation.Value).Any(threat => threat is Slime))
			{
				var newProgeny = CreateProgeny(stationLocation.Value);
				newProgeny.Initialize(SittingDuck, ThreatController, TimeAppears);
				currentProgeny.Add(newProgeny);
				SittingDuck.AddInternalThreatToStations(new [] {stationLocation.Value}, newProgeny);
				newProgeny.PlaceOnTrack(Track, Position.GetValueOrDefault());
				ThreatController.InternalThreats.Add(newProgeny);
			}
		}
	}
}
