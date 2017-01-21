using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow.Slime
{
	public class ProgenySlimeA : ProgenySlime
	{
		public ProgenySlimeA(NormalSlime parent, StationLocation stationLocation)
			: base(parent, stationLocation)
		{
		}

		public override string Id { get; } = "I2-01";
		public override string DisplayName { get; } = "Slime";
		public override string FileName { get; } = "SlimeA";
	}
}
