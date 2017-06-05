using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitterB : HiddenTransmitter
	{
		internal HiddenTransmitterB() : base(StationLocation.UpperBlue)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainShields(new [] {CurrentZone});
			Attack(4);
		}

		public override string Id { get; } = "SI3-102";
		public override string DisplayName { get; } = "Hidden Transmitter";
		public override string FileName { get; } = "HiddenTransmitterB";
	}
}
