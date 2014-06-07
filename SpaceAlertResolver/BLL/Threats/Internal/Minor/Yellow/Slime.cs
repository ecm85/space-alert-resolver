using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Slime : MinorYellowInternalThreat
	{
		//TODO: Players entering location with slime are delayed

		private readonly IList<Slime> currentProgeny;

		protected Slime(int timeAppears, StationLocation currentStation, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, currentStation, PlayerAction.BattleBots, sittingDuck)
		{
			currentProgeny = new List<Slime>();
		}

		public override void PerformZAction()
		{
			Damage(2);
		}

		public override bool IsDefeated
		{
			get { return base.IsDefeated && currentProgeny.All(progeny => progeny.IsDefeated); }
		}

		protected abstract Slime CreateProgeny();

		protected void Spread(StandardStation station)
		{
			if (station != null && !station.Threats.Any(threat => threat is Slime))
			{
				var newProgeny = CreateProgeny();
				currentProgeny.Add(newProgeny);
				station.Threats.Add(CreateProgeny());
				Track.ThreatPositions[newProgeny] = Track.ThreatPositions[this];
				sittingDuck.CurrentInternalThreats.Add(newProgeny);
				newProgeny.SetTrack(Track);
			}
		}
	}
}
