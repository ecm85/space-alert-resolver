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

		private class ProgenySlime : SlimeB, IPseudoThreat
		{
			public ProgenySlime(SlimeB parent, StationLocation stationLocation)
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

			public SlimeB ParentSlime { get; }
			public Threat Parent { get; }
		}

		protected override Slime CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlime(this, stationLocation);
		}
	}
}
