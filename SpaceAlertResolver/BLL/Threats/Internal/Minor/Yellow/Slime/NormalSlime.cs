using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow.Slime
{
	public abstract class NormalSlime : BaseSlime
	{
		protected NormalSlime(StationLocation stationLocation) : base(2, stationLocation)
		{
			Progeny = new List<ProgenySlime>();
		}

		protected override void PerformYAction(int currentTurn)
		{
			SpreadFrom(CurrentStation, Position);
		}

		private IList<ProgenySlime> Progeny { get; }

		public override IList<StationLocation> DisplayStations
		{
			get
			{
				return base.DisplayStations
					.Concat(Progeny.Where(progeny => progeny.IsOnShip).SelectMany(progeny => progeny.CurrentStations))
					.ToList();
			}
		}

		protected override bool IsDefeatedWhenHealthReachesZero { get { return Progeny.All(progeny => !progeny.IsOnShip); } }

		public void OnProgenyKilled()
		{
			if (!IsOnShip && Progeny.All(progeny => !progeny.IsOnShip))
				SetThreatStatus(ThreatStatus.Defeated, true);
		}

		protected abstract ProgenySlime CreateProgeny(StationLocation stationLocation);

		public void SpreadFrom(StationLocation spreadFromStation, int position)
		{
			var spreadToStation = GetStationToSpreadTo(spreadFromStation);
			if (spreadToStation == null ||
			    ThreatController.DamageableInternalThreats.Any(threat => threat is BaseSlime && threat.CurrentStation == spreadToStation))
				return;
			var newProgeny = CreateProgeny(spreadToStation.Value);
			Progeny.Add(newProgeny);
			newProgeny.Initialize(SittingDuck, ThreatController);
			ThreatController.AddInternalThreat(newProgeny, TimeAppears, position);
		}

		protected abstract StationLocation? GetStationToSpreadTo(StationLocation stationLocation);
	}
}
