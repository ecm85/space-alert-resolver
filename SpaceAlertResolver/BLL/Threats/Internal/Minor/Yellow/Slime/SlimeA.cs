using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow.Slime
{
	public class SlimeA : NormalSlime
	{
		internal SlimeA()
			: base(StationLocation.LowerBlue)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.RemoveRocket();
		}

		public override string Id { get; } = "I2-01";
		public override string DisplayName { get; } = "Slime";
		public override string FileName { get; } = "SlimeA";

		protected override ProgenySlime CreateProgeny(StationLocation stationLocation)
		{
			return new ProgenySlimeA(this, stationLocation);
		}

		protected override StationLocation? GetStationToSpreadTo(StationLocation stationLocation)
		{
			return stationLocation.RedwardStationLocation();
		}
	}
}
