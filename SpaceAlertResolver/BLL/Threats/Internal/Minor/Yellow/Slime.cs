using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Slime : MinorYellowInternalThreat
	{
		//TODO: Players entering location with slime are delayed, and this effect persists past z

		private readonly IList<Slime> currentProgeny;

		protected Slime(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerAction.BattleBots)
		{
			currentProgeny = new List<Slime>();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}

		public override bool IsDefeated
		{
			get { return base.IsDefeated && currentProgeny.All(progeny => progeny.IsDefeated); }
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
