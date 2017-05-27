using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class HackedShieldsA : HackedShields
	{
		internal HackedShieldsA()
			: base(StationLocation.UpperRed)
		{
		}

		public override string Id { get; } = "I1-06";
		public override string DisplayName { get; } = "Hacked Shields";
		public override string FileName { get; } = "HackedShieldsA";
	}
}
