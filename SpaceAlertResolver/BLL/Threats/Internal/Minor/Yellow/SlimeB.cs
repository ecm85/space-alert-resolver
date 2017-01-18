using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeB : Slime
	{
		public SlimeB()
			: base(StationLocation.LowerRed)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (!base.IsDefeated)
				SittingDuck.DisableLowerRedInactiveBattleBots();
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (!base.IsDefeated)
				Spread(CurrentStation.BluewardStationLocation());
		}

		protected override void PerformZAction(int currentTurn)
		{
			if (!base.IsDefeated)
				Damage(2);
		}

		public override bool IsDefeated
		{
			get { return base.IsDefeated && CurrentProgeny.All(progeny => progeny.IsDefeated); }
		}

		public override string Id { get; } = "I2-02";
		public override string DisplayName { get; } = "Slime";
		public override string FileName { get; } = "SlimeB";

		private class ProgenySlimeB : ProgenySlime
		{
			public ProgenySlimeB(SlimeB parent, StationLocation stationLocation)
				: base(1, 2, stationLocation, PlayerActionType.BattleBots)
			{
				ParentSlime = parent;
			}

			protected override void PerformYAction(int currentTurn)
			{
				ParentSlime.Spread(CurrentStation.BluewardStationLocation());
			}

			public override string Id { get; } = "I2-01";
			public override string DisplayName { get; } = "Slime";
			public override string FileName { get; } = "SlimeA";
		}

		protected override MinorYellowInternalThreat CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlimeB(this, stationLocation);
		}
	}
}
