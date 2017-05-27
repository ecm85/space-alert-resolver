using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Red
{
	public class HiddenTransmitterA : HiddenTransmitter
	{
		internal HiddenTransmitterA() : base(StationLocation.LowerRed)
		{
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone);
			Damage(4);
		}

		public override string Id { get; } = "SI3-103";
		public override string DisplayName { get; } = "Hidden Transmitter";
		public override string FileName { get; } = "HiddenTransmitterA";
	}
}
