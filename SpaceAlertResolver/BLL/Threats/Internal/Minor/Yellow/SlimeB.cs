using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeB : Slime
	{
		public SlimeB()
			: base(StationLocation.LowerRed)
		{
		}

		private SlimeB(int health, StationLocation stationLocation)
			: base(health, stationLocation)
		{
		}


		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DisableLowerRedInactiveBattleBots();
		}

		protected override void PerformYAction(int currentTurn)
		{
			Spread(CurrentStation.BluewardStationLocation());
		}

		public override string Id { get; } = "I2-02";
		public override string DisplayName { get; } = "Slime";
		public override string FileName { get; } = "SlimeB";

		private class ProgenySlime : SlimeB
		{
			public ProgenySlime(SlimeB parent, StationLocation stationLocation)
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
				ParentSlime.Spread(CurrentStation.BluewardStationLocation());
			}

			public SlimeB ParentSlime { get; }
		}

		protected override Slime CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlime(this, stationLocation);
		}
	}
}
