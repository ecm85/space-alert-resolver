using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class SkirmishersA : Skirmishers
	{
		public SkirmishersA()
			: base(StationLocation.UpperRed)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			MoveBlue();
		}
	}
}
