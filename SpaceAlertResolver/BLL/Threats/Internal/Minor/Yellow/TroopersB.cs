using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class TroopersB : Troopers
	{
		internal TroopersB()
			: base(StationLocation.UpperRed)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveBlue();
		}

		public override string Id { get; } = "I2-03";
		public override string DisplayName { get; } = "Troopers";
		public override string FileName { get; } = "TroopersB";
	}
}
