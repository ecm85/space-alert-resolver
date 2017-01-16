using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class HackedShieldsB : HackedShields
	{
		public HackedShieldsB()
			: base(StationLocation.UpperBlue)
		{
		}

		public override string Id { get; } = "I1-05";
		public override string DisplayName { get; } = "Hacked Shields";
		public override string FileName { get; } = "HackedShieldsB";
	}
}
