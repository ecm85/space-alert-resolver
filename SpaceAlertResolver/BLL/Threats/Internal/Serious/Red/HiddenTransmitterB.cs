using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitterB : HiddenTransmitter
	{
		public HiddenTransmitterB() : base(StationLocation.UpperBlue)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainShield(CurrentZone);
			Damage(4);
		}

		public override string Id { get; } = "SI3-102";
		public override string DisplayName { get; } = "Hidden Transmitter";
		public override string FileName { get; } = "HiddenTransmitterA";
	}
}
