using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class SlimeA : Slime
	{
		public SlimeA()
			: base(StationLocation.LowerBlue)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (!base.IsDefeated)
				SittingDuck.RemoveRocket();
		}

		protected override void PerformYAction(int currentTurn)
		{
			if (!base.IsDefeated)
				Spread(CurrentStation.RedwardStationLocation());
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

		public override string Id { get; } = "I2-01";
		public override string DisplayName { get; } = "Slime";
		public override string FileName { get; } = "SlimeA";

		private class ProgenySlimeA : ProgenySlime
		{
			public ProgenySlimeA(SlimeA parent, StationLocation stationLocation)
				: base(1, 2, stationLocation, PlayerActionType.BattleBots)
			{
				ParentSlime = parent;
			}

			protected override void PerformYAction(int currentTurn)
			{
				ParentSlime.Spread(CurrentStation.RedwardStationLocation());
			}

			public override string Id { get; } = "I2-01";
			public override string DisplayName { get; } = "Slime";
			public override string FileName { get; } = "SlimeA";
		}

		protected override MinorYellowInternalThreat CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlimeA(this, stationLocation);
		}
	}
}
