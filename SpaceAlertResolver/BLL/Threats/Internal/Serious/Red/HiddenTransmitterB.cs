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
	}
}
