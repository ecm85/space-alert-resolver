using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeA : Slime
	{
		public SlimeA()
			: base(StationLocation.LowerBlue)
		{
		}

		private SlimeA(int health, StationLocation stationLocation)
			: base(health, stationLocation)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.RemoveRocket();
		}

		protected override void PerformYAction(int currentTurn)
		{
			var redwardStation = CurrentStation.RedwardStationLocation();
			Spread(redwardStation);
		}

		private class ProgenySlime : SlimeA, IPseudoThreat
		{
			public ProgenySlime(SlimeA parent, StationLocation stationLocation)
				: base(1, stationLocation)
			{
				Parent = parent;
				ParentSlime = parent;
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

			protected override void PerformYAction(int currentTurn)
			{
				ParentSlime.Spread(CurrentStation.BluewardStationLocation());
			}

			public Threat Parent { get; }
			public SlimeA ParentSlime { get; set; }
		}

		protected override Slime CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlime(this, stationLocation);
		}
	}
}
