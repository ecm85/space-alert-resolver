using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public class SkirmishersB : Skirmishers
	{
		public SkirmishersB()
			: base(StationLocation.UpperBlue)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			MoveRed();
		}
	}
}
