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
			Spread(CurrentStation.RedwardStationLocation());
		}

		public override string Id { get; } = "I2-01";
		public override string DisplayName { get; } = "Slime";
		public override string FileName { get; } = "SlimeA";

		private class ProgenySlime : SlimeA
		{
			public ProgenySlime(SlimeA parent, StationLocation stationLocation)
				: base(1, stationLocation)
			{
				ParentSlime = parent;
			}

			public override bool ShowOnTrack { get { return false; } }

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
				ParentSlime.Spread(CurrentStation.RedwardStationLocation());
			}

			public SlimeA ParentSlime { get; set; }
		}

		protected override Slime CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlime(this, stationLocation);
		}
	}
}
