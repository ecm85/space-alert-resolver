using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class TroopersA : Troopers
	{
		public TroopersA()
			: base(StationLocation.LowerBlue)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveRed();
		}

		public override string Id { get; } = "I2-04";
		public override string DisplayName { get; } = "Troopers";
		public override string FileName { get; } = "TroopersA";
	}
}
